using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Dtos.AboutItemDtos;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Controllers;

public class AboutItemController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public AboutItemController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult AboutItemList()
    {
        var aboutItems = _context.AboutItems.ToList();
        var result = _mapper.Map<List<ResultAboutItemDto>>(aboutItems);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateAboutItem()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAboutItem(CreateAboutItemDto caidto)
    {
        var aboutItem = _mapper.Map<AboutItem>(caidto);
        _context.AboutItems.Add(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }

    public IActionResult DeleteAboutItem(int id)
    {
        var aboutItem = _context.AboutItems.Find(id);
        _context.AboutItems.Remove(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }

    [HttpGet]
    public IActionResult UpdateAboutItem(int id)
    {
        var aboutItem = _context.AboutItems.Find(id);
        var result = _mapper.Map<UpdateAboutItemDto>(aboutItem);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateAboutItem(UpdateAboutItemDto uaidto)
    {
        var aboutItem = _mapper.Map<AboutItem>(uaidto);
        _context.AboutItems.Update(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }

    [HttpGet]
    public async Task<IActionResult> CreateAboutItemwithGoogleGemini()
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
                            text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel 'Hakkımızda Alanları (about item)' yazısı oluştur. Örneğin: 'Geleceğinizi güvence altına alan kapsamlı sigorta çözümleri sunuyoruz.' şeklinde veya bunun gibi ve buna benzer daha zengin içerikler gelsin. En az 10 tane item istiyorum.",
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
        var aboutItemText = jsonDoc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        ViewBag.aboutItemText = aboutItemText;

        return View();
    }
}
