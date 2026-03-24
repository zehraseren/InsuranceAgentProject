using AutoMapper;
using System.Text;
using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.AboutDtos;

namespace InsureYouAI.Controllers;

public class AboutController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public AboutController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult AboutList()
    {
        var abouts = _context.Abouts.ToList();
        var result = _mapper.Map<List<ResultAboutDto>>(abouts);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateAbout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAbout(CreateAboutDto cadto)
    {
        var about = _mapper.Map<About>(cadto);
        _context.Abouts.Add(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }

    public IActionResult DeleteAbout(int id)
    {
        var about = _context.Abouts.Find(id);
        _context.Abouts.Remove(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }

    [HttpGet]
    public IActionResult UpdateAbout(int id)
    {
        var about = _context.Abouts.Find(id);
        var result = _mapper.Map<UpdateAboutDto>(about);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateAbout(UpdateAboutDto uadto)
    {
        var about = _mapper.Map<About>(uadto);
        _context.Abouts.Update(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }

    [HttpGet]
    public async Task<IActionResult> CreateAboutWithGoogleGemini()
    {
        var apiKey = "YOUR_API_KEY_HERE";
        var model = "gemini-2.5-flash";
        var url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel 'Hakkımızda' yazısı oluştur.",
                        }
                    }
                }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(url, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        using var jsonDoc = JsonDocument.Parse(responseJson);
        var aboutText = jsonDoc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        ViewBag.aboutText = aboutText;

        return View();
    }
}
