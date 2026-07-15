namespace InsureYouAI.Models;

public class PolicyAnalysisViewModel
{
    public IFormFile? PdfFile { get; set; }
    public string? AnalysisResult { get; set; }
    public string? Error { get; set; }
}
