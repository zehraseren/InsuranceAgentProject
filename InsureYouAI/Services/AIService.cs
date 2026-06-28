using System.Text;
using System.Text.Json;
using InsureYouAI.Dtos.MessageDtos;

namespace InsureYouAI.Services;

public class AIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "YOUR_API_KEY_HERE";
    private readonly string _model = "gemini-2.5-flash";

    public AIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Kullanıcı mesajını analiz eder
    public async Task<AIClassificationResult> AnalyzeMessageAsync(string messageText)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

        var prompt = BuildPrompt(messageText);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        // JSON serialize → API'ye gönderilecek payload
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // API çağrısı
        var response = await _httpClient.PostAsync(url, content);

        // API başarısızsa fallback döndür (sistem çökmesin diye)
        if (!response.IsSuccessStatusCode) return Fallback();

        // Response body okuma
        var result = await response.Content.ReadAsStringAsync();

        // JSON içinden model çıktısı çekilmesi
        var rawText = ExtractText(result);

        // Model çıktısı parse edilmesi (Category)
        return ParseResult(rawText);
    }

    // AI'a gönderilecek prompt'u üretme
    // (Burada modelden STRICT output beklenir)
    private string BuildPrompt(string messageText)
    {
        return $"""
Aşağıdaki sigorta mesajını analiz et.

ÇIKTI FORMATI : Category

Kategori seçenekleri:
Kasko, Trafik Sigortası, Sağlık Sigortası, Konut Sigortası, Hasar Bildirimi, Fiyat Teklifi, Poliçe Yenileme, Genel Soru, İletişim Talebi

Kurallar:
- Sadece tek satır döndür
- Açıklama yok
- Markdown yok
- Kod bloğu yok

Mesaj:
{messageText}
""";
    }

    // Gemini API response içinden text alanına çıkarması
    private string ExtractText(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";
        }
        catch
        {
            return "";
        }
    }

    // Model çıktısını Category'e ayırması 
    // Beklenen format: Category
    private AIClassificationResult ParseResult(string text)
    {
        // Boş veya hatalı output gelirse fallback'e düşmesi
        if (string.IsNullOrWhiteSpace(text)) return Fallback();

        var category = text
            .Replace("```", "")
            .Trim();

        return new AIClassificationResult
        {
            Category = category
        };
    }

    // Sistem güvenli fallback mekanizması
    // AI cevap veremezse default değerler döner
    private AIClassificationResult Fallback()
    {
        return new AIClassificationResult
        {
            Category = "Genel Soru"
        };
    }
}