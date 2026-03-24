using AutoMapper;
using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ServiceDtos;

namespace InsureYouAI.Controllers;

public class ServiceController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public ServiceController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult ServiceList()
    {
        var services = _context.Services.ToList();
        var result = _mapper.Map<List<ResultServiceDto>>(services);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateService()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateService(CreateServiceDto csdto)
    {
        var service = _mapper.Map<Service>(csdto);
        _context.Services.Add(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }

    public IActionResult DeleteService(int id)
    {
        var service = _context.Services.Find(id);
        _context.Services.Remove(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }

    [HttpGet]
    public IActionResult UpdateService(int id)
    {
        var service = _context.Services.Find(id);
        var result = _mapper.Map<UpdateServiceDto>(service);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateService(UpdateServiceDto usdto)
    {
        var service = _mapper.Map<Service>(usdto);
        _context.Services.Update(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }

    public async Task<IActionResult> CreateServiceWithAnthropicClaude()
    {
        var apiKey = "YOUR_API_KEY_HERE";
        string prompt = "Bir sigorta şirketi için hizmetler bölümü hazırlamanı istiyorum. Burada 5 darklı hizmet olmalı. Bana maksimum 100 karakterden oluşan cümlelerle 5 tane hizmet içeriği yazar mısın?";

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.anthropic.com/");
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestBody = new
        {
            model = "claude-3-5-20241022",
            max_tokens = 512,
            temperature = 0.5,
            messages = new[]
            {
                new
                {
                    role="user",
                    content = prompt
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody));
        var response = await client.PostAsync("v1/messages", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.services = new List<string>
            {
                $"Claude API'dan cevap alınamadı: {response.StatusCode}"
            };
            return View();
        }
        var responseString = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseString);
        var fullText = doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString();

        var services = fullText.Split('\n')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.TrimStart('1', '2', '3', '4', '5', '.', ' '))
            .ToList();

        return View();
    }
}
