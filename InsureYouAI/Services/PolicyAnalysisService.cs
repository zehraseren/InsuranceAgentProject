using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services;

public class PolicyAnalysisService
{
    private readonly HttpClient _httpClient;
    private readonly string apiKey = "YOUR_API_KEY_HERE";
    private readonly string model = "gemini-2.5-flash";

    public PolicyAnalysisService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AnalyzePolicyAsync(string base64Pdf)
    {
        var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        var prompt = @"Bu PDF bir sigorta poliçesine aittir.

Bu PDF bir sigorta poliçesidir.

Kurallar:

- Her madde en fazla 1 cümle olsun.
- Toplam çıktı 700 kelimeyi geçmesin.
- Gereksiz açıklama yapma.
- Tekrar eden bilgi verme.
- Aynı bilgiyi iki farklı bölümde yazma.
- Markdown kullan.

İstenen çıktı:

# 1) Poliçeyi Özetle
Tam 10 madde.

# 2) Neleri Kapsar
En fazla 10 madde.

# 3) Neleri Kapsamaz
En fazla 10 madde.

# 4) Kritik Uyarılar
En fazla 5 madde.";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = prompt },
                        new { inline_data = new { mime_type= "application/pdf", data = base64Pdf } }
                    }
                }
            },
            generationConfig = new { temperature = 0.2, maxOutputTokens = 8192 }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(apiUrl, content);
        var responseText = await response.Content.ReadAsStringAsync();

        var path = Path.Combine(Directory.GetCurrentDirectory(), "response.json");
        File.WriteAllText(path, responseText);

        if (!response.IsSuccessStatusCode)
            return $"AI Analizi sırasında hata oluştu: {response.StatusCode}\n{responseText}";

        using var doc = JsonDocument.Parse(responseText);

        if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
        {
            return candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? string.Empty;
        }

        return "AI'dan geçerli bir yanıt alınamadı.";
    }
}
