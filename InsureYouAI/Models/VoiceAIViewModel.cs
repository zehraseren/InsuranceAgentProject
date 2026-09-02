using InsureYouAI.Enums;

namespace InsureYouAI.Models;

public class VoiceAIViewModel
{
    public string? Text { get; set; }
    public VoiceResponseMode Mode { get; set; }
    public string? AudioUrl { get; set; }
    public string? Error { get; set; }
}
