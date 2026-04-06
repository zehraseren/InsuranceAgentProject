using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class AIImageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AIImageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult GenerateImageGemini()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerateImageGemini(string prompt)
    {
        var apiKey = "YOUR_API_KEY_HERE";
        var client = _httpClientFactory.CreateClient();

        var modelName = "imagen-4.0-fast-generate-001";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:predict?key={apiKey}";

        var requestData = new
        {
            instances = new[] { new { prompt = prompt } },
            parameters = new
            {
                sampleCount = 1,
                aspectRatio = "1:1"
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            ViewBag.error = $"Google API Mesajı: {errorBody}";
            return View();
        }

        var json = await response.Content.ReadAsStringAsync();
        using var result = JsonDocument.Parse(json);

        if (result.RootElement.TryGetProperty("predictions", out var predictions))
        {
            var base64Image = predictions[0].GetProperty("bytesBase64Encoded").GetString();
            var imageUrl = $"data:image/png;base64,{base64Image}";

            return View(model: imageUrl);
        }

        ViewBag.error = "Görsel oluşturulamadı, API yanıtı boş döndü.";
        return View();
    }
}
