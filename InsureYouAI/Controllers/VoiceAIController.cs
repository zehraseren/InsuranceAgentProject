using InsureYouAI.Models;
using InsureYouAI.Services;
using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Enums;

namespace InsureYouAI.Controllers;

public class VoiceAIController : Controller
{
    private readonly VoiceAIService _voiceAIService;

    public VoiceAIController(VoiceAIService voiceAIService)
    {
        _voiceAIService = voiceAIService;
    }

    [HttpGet]
    [PageInfo("Sesli Asistan", "Sesli Yapay Zeka Asistanı")]
    public IActionResult TextToSpeech()
    {
        return View(new VoiceAIViewModel { Mode = VoiceResponseMode.Player });
    }

    [HttpGet]
    [PageInfo("Sesli Asistan", "Sesli Yapay Zeka Asistanı - Avatar")]
    public IActionResult TextToSpeechWithAvatar()
    {
        return View("TextToSpeechWithAvatar", new VoiceAIViewModel { Mode = VoiceResponseMode.Avatar });
    }

    [HttpPost]
    public async Task<IActionResult> TextToSpeech(VoiceAIViewModel vaivm)
    {
        var viewName = vaivm.Mode == VoiceResponseMode.Avatar
             ? "TextToSpeechWithAvatar"
             : "TextToSpeech";

        if (string.IsNullOrWhiteSpace(vaivm.Text))
        {
            vaivm.Error = "Lütfen bir metin giriniz.";
            return View(viewName, vaivm);
        }

        try
        {
            vaivm.AudioUrl = await _voiceAIService.GenerateSpeechAsync(vaivm.Text);
        }
        catch (InvalidOperationException ex)
        {
            vaivm.Error = $"Ses oluşturulamadı: {ex.Message}";
        }

        return View(viewName, vaivm);
    }

    [HttpGet]
    [PageInfo("Sesli Asistan", "Sesli Yanıt & Akıllı Sohbet Alanı")]
    public IActionResult ChatWithVoiceAI()
    {
        return View(new VoiceChatViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> ChatWithVoiceAI(VoiceChatViewModel vcvm)
    {
        if (string.IsNullOrWhiteSpace(vcvm.Text))
        {
            vcvm.Answer = "Lütfen bir soru veya metin giriniz.";
            return View(vcvm);
        }

        vcvm.Answer = await _voiceAIService.GenerateTextAnswerAsync(vcvm.Text);
        try
        {
            vcvm.AudioUrl = await _voiceAIService.GenerateSpeechAsync(vcvm.Answer);
        }
        catch (InvalidOperationException ex)
        {
            vcvm.Error = $"Yanıt oluşturulamadı: {ex.Message}";
        }

        return View(vcvm);
    }
}
