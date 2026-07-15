using InsureYouAI.Models;
using InsureYouAI.Services;
using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class PolicyAnalysisAIController : Controller
{
    private readonly PolicyAnalysisService _policyAnalysisService;

    public PolicyAnalysisAIController(PolicyAnalysisService policyAnalysisService)
    {
        _policyAnalysisService = policyAnalysisService;
    }

    [HttpGet]
    [PageInfo("Poliçe Analizi", "Google Gemini ile PDF Analizi")]
    public IActionResult AnalyzePdf()
    {
        return View(new PolicyAnalysisViewModel());
    }

    [HttpPost]
    [PageInfo("Poliçe Analizi", "Google Gemini ile PDF Analizi")]
    public async Task<IActionResult> AnalyzePdf(PolicyAnalysisViewModel pavm)
    {
        if (pavm.PdfFile == null || pavm.PdfFile.Length == 0)
        {
            pavm.Error = "Lütfen bir PDF poliçe dosyası yükleyiniz.";
            return View(pavm);
        }

        if (pavm.PdfFile.ContentType != "application/pdf")
        {
            pavm.Error = "Lütfen geçerli bir PDF dosyası yükleyiniz.";
            return View(pavm);
        }

        using var ms = new MemoryStream();
        await pavm.PdfFile.CopyToAsync(ms);
        var base64Pdf = Convert.ToBase64String(ms.ToArray());

        pavm.AnalysisResult = await _policyAnalysisService.AnalyzePolicyAsync(base64Pdf);

        return View(pavm);
    }
}
