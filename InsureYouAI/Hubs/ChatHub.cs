using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;

namespace InsureYouAI.Hubs;

public class ChatHub : Hub
{
    private const string apiKey = "YOUR_API_KEY_HERE";
    private const string modelGroq = "llama-3.3-70b-versatile";
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatHub(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Her bağlı kullanıcının konuşma geçmişini tutan yapı
    // Anahtar: SignalR'ın her bağlantıya verdiği benzersiz ID (ConnectionId)
    // Değer: O kullanıcıya ait mesaj listesi (role + content çiftleri)
    // Static olması kritik: Hub instance'ları request başına oluşturulur, static olmazsa her mesajda geçmiş sıfırlanır.
    private static readonly Dictionary<string, List<Dictionary<string, string>>> _history = new();

    // Kullanıcı bağlandığında tetiklenir.
    // ConnectionId'ye karşılık yeni bir geçmiş oluşturulur.
    // Sistem mesajı eklenerek AI'a rolü tanıtılır.
    public override Task OnConnectedAsync()
    {
        _history[Context.ConnectionId] =
            [
                new()
                {
                    ["role"] = "system",
                    ["content"] = "You are a helpful assistant. Keep answer concise."
                }
            ];
        return base.OnConnectedAsync();
    }

    // Kullanıcı bağlantıyı kestiğinde tetiklenir.
    // Bellekte tutulan geçmiş temizlenir, bellek sızıntısı önlenir.
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _history.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    // Client tarafından çağrılan metot.
    // Kullanıcının yazdığı mesajı önce ekranda gösterir (echo), ardından geçmişe ekleyip AI'a gönderir.
    public async Task SendMessage(string userMessage)
    {
        // Kullanıcının mesajını anında kendisine geri gönder (ekranda görünsün)
        await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);

        // Bu kullanıcının geçmişini al ve yeni mesajı ekle
        var history = _history[Context.ConnectionId];
        history.Add(new() { ["role"] = "user", ["content"] = userMessage });

        // Groq'a gönder, cevabı stream et
        await StreamGroq(history, Context.ConnectionAborted);
        //await StreamGroq(history, CancellationToken.None);
    }

    // Groq API'ye istek gönderir ve gelen yanıtı token token client'a iletir.
    // "stream: true" sayesinde yanıt tek seferde değil parça parça gelir, bu sayede kullanıcı cevabın yazılmasını gerçek zamanlı izler.
    private async Task StreamGroq(List<Dictionary<string, string>> history, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("groq");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var payload = new
        {
            model = modelGroq,
            messages = history, // Tüm geçmiş gönderilir, AI konuşma bağlamını bu sayede hatırlar
            stream = true,      // Streaming modu: yanıt parça parça gelir
            temperature = 0.2   // Düşük temperature = daha tutarlı ve odaklı yanıtlar
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        // ResponseHeadersRead: tüm body beklenmeden header gelince okumaya başla (streaming için şart)
        using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            await Clients.Caller.SendAsync("ReceiveError", errorText);
            return;
        }

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        var stringBuilder = new StringBuilder();
        // Groq, SSE (Server-Sent Events) formatında yanıt döner.
        // Her satır "data: {...}" şeklinde gelir, [DONE] ile biter.
        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (!line.StartsWith("data: ")) continue;

            // "data: " prefix'ini kaldır, saf JSON'ı al
            var data = line["data:".Length..].Trim();
            if (data == "[DONE]") break;

            try
            {
                // Gelen JSON chunk'ını deserialize et, içindeki delta content'i al
                var chunk = JsonSerializer.Deserialize<ChatStreamChunk>(data);
                var delta = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;
                if (!string.IsNullOrEmpty(delta))
                {
                    stringBuilder.AppendLine(delta);
                    // Token'ı anında client'a gönder (yazıyor efekti)
                    await Clients.Caller.SendAsync("ReceiveToken", delta, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                // Bazı chunk'lar bozuk veya beklenmedik formatta gelebilir.
                // Tek bir chunk hatası tüm stream'i patlatmasın, sessizce geç.
                await Clients.Caller.SendAsync("ReceiveError", ex.Message, cancellationToken);
            }
        }

        // Tüm token'lar bitti, tam yanıtı geçmişe ekle
        var fullResponse = stringBuilder.ToString();
        history.Add(new() { ["role"] = "assistant", ["content"] = fullResponse });
        // Client'a "mesaj tamamlandı" sinyali gönder
        await Clients.Caller.SendAsync("CompleteMessage", fullResponse, cancellationToken);
    }

    // Groq'tan gelen streaming chunk'ının JSON yapısı
    private sealed class ChatStreamChunk
    {
        [JsonPropertyName("choices")] public List<Choice>? Choices { get; set; }
    }

    private sealed class Choice
    {
        [JsonPropertyName("delta")] public Delta? Delta { get; set; }
        [JsonPropertyName("finish_reason")] public string? FinishReason { get; set; }
    }

    private sealed class Delta
    {
        [JsonPropertyName("content")] public string? Content { get; set; }
        [JsonPropertyName("role")] public string? Role { get; set; }
    }
}

/*
 * ÖNEMLİ NOTLAR
 * =============
 *
 * 1. NEDEN static _history?
 *    SignalR, Hub sınıfını her mesaj geldiğinde yeniden oluşturur (transient).
 *    Eğer _history static olmasaydı her mesajda sıfırlanır, AI önceki konuşmayı
 *    hatırlamazdı. static sayesinde uygulama ayakta kaldığı sürece geçmiş korunur.
 *
 * 2. NEDEN tüm geçmiş API'ye gönderilir?
 *    Groq (ve diğer LLM API'leri) stateless çalışır, yani önceki mesajları
 *    kendisi hatırlamaz. Bağlamı korumak için her istekte tüm konuşma geçmişini
 *    göndermek zorundasın. Bu yüzden history listesi büyüdükçe token kullanımı artar.
 *
 * 3. NEDEN stream = true?
 *    Streaming kapalı olsaydı AI cevabın tamamını üretene kadar bekler,
 *    sonra hepsini bir anda gönderirdi. Streaming ile token token geliyor,
 *    kullanıcı cevabın yazıldığını gerçek zamanlı görüyor — chatbot hissi bu sayede oluşuyor.
 *
 * 4. SSE (Server-Sent Events) Formatı nedir?
 *    Groq yanıtı şu formatta gönderir:
 *    data: {"choices":[{"delta":{"content":"Merhaba"},...}]}
 *    data: {"choices":[{"delta":{"content":" nasıl"},...}]}
 *    data: [DONE]
 *    Her satırı okuyup "data: " prefix'ini kaldırarak JSON parse ediyoruz.
 *
 * 5. CompleteMessage neden var?
 *    ReceiveToken ile token token gönderilen mesaj ekranda birikiyor.
 *    CompleteMessage ile "stream bitti" sinyali veriliyor, client tarafında
 *    örneğin loading spinner kapatmak veya mesajı geçmişe kaydetmek için kullanılır.
 */