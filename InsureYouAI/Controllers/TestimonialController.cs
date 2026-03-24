using AutoMapper;
using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.TestimonialDtos;

namespace InsureYouAI.Controllers;

public class TestimonialController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public TestimonialController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult TestimonialList()
    {
        var testimonials = _context.Testimonials.ToList();
        var result = _mapper.Map<List<ResultTestimonialDto>>(testimonials);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateTestimonial()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTestimonial(CreateTestimonialDto ctdto)
    {
        var testimonial = _mapper.Map<Testimonial>(ctdto);
        _context.Testimonials.Add(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }

    public IActionResult DeleteTestimonial(int id)
    {
        var testimonial = _context.Testimonials.Find(id);
        _context.Testimonials.Remove(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }

    [HttpGet]
    public IActionResult UpdateTestimonial(int id)
    {
        var testimonial = _context.Testimonials.Find(id);
        var result = _mapper.Map<UpdateTestimonialDto>(testimonial);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateTestimonial(UpdateTestimonialDto utdto)
    {
        var testimonial = _mapper.Map<Testimonial>(utdto);
        _context.Testimonials.Update(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }

    public async Task<IActionResult> CreateTestimonialWithClaudeAI()
    {
        var apiKey = "YOUR_API_KEY_HERE";
        string prompt = "Bir sigorta şirketi için müşteri deneyimlerine dair yorum oluşturmak istiyorum yani İngilizce karşılığı ile: testimonial. Bu alanda Türkçe olarak 6 tane yorum, 6 tane müşteri adı ve soyadı, bu müşterilerin unvanı olsun. Buna göre içeriği hazırla.";

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
            ViewBag.testimonials = new List<string>
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

        var testimonials = fullText.Split('\n')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.TrimStart('1', '2', '3', '4', '5', '.', ' '))
            .ToList();

        return View();
    }
}
