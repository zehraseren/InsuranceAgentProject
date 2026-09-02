using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services;

public class VoiceAIService
{
    private readonly HttpClient _httpClient;

    private const string _apiKey = "YOUR_API_KEY_HERE";
    private const string VoiceId = "EXAVITQu4vr4xnSDxMaL"; // Rachel
    private const string ModelId = "eleven_multilingual_v2";
    public VoiceAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GenerateSpeechAsync(string text)
    {
        var url = $"https://api.elevenlabs.io/v1/text-to-speech/{VoiceId}/stream";

        var requestBody = new
        {
            text = text,
            model_id = ModelId,
            voice_settings = new { stability = 0.5, similarity_boost = 0.8 }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("xi-api-key", _apiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Ses oluşturulamadı. Status: {response.StatusCode}, Detay: {errorBody}");
        }

        var audioBytes = await response.Content.ReadAsByteArrayAsync();

        var fileName = $"voice_{Guid.NewGuid()}.mp3";
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "voices");
        Directory.CreateDirectory(directory);

        await File.WriteAllBytesAsync(Path.Combine(directory, fileName), audioBytes);

        return $"/voices/{fileName}";
    }

    public Task<string> GenerateTextAnswerAsync(string userText)
    {
        return Task.FromResult($"InsureYOU AI yanıtı: {userText}");
    }
}
