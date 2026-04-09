using AutoMapper;
using System.Text;
using System.Text.Json;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.AppUserDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace InsureYouAI.Controllers;

public class AppUserController : Controller
{
    private readonly IMapper _mapper;
    private readonly InsureContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpClientFactory _httpClientFactory;

    public AppUserController(IMapper mapper, InsureContext context, UserManager<AppUser> userManager, IHttpClientFactory httpClientFactory)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult UserList()
    {
        var users = _userManager.Users.ProjectTo<ResultAppUserDto>(_mapper.ConfigurationProvider).ToList();

        return View(users);
    }

    public async Task<IActionResult> UserAIAnalysis(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var userProfile = _mapper.Map<GetAppUserProfileDto>(user);

        // Kullanıcıya ait makaleler
        var articles = await _context.Articles
            .Where(au => au.AppUserId == id)
            .Select(a => a.Content)
            .ToListAsync();

        if (articles.Count == 0)
        {
            ViewBag.AIResult = "Bu kullanıcıya ait analiz yapılacak makale bulunamadı!";
            return View(userProfile);
        }

        // Makaleleri tek bir metin halinde getirme
        var allArticles = string.Join("\n\n", articles);

        var apiKey = "YOUR_API_KEY_HERE";

        // Prompt oluşturma
        var prompt = $@"
            Siz bir sigorta sektöründe uzman bir içerik analistisin.
            Elinizde, bir sigorta şirketinin çalışanının yazdığı tüm makaleler var.
            Bu makaleler üzerinden çalışanın içerik üretim tarzını analiz et.

            Analiz Başlıkları:

            1) Konu çeşitliliği ve odak alanları (sağlık, hayat, kasko, tamamlayıcı, BES vb.)
            2) Hedef kitle tahmini (bireysel/kurumsal, segment, persona)
            3) Dil ve Anlatım Tarzı (tekniklik seviyesi, okunabilirlik, ikna gücü)
            4) Sigorta terimlerini kullanma ve doğruluk düzeyi
            5) Müşteri ihtiyaçlarına ve risk yönetimine odaklanma
            6) Pazarlama/satış vurgusu, CTA netliği
            7) Geliştirilmesi gereken alanlar ve net aksiyon maddeleri

            Makaleler:

            {allArticles}

            Lütfen çıktıyı profesyonel rapor formatında, madde madde ve en sonda 5 maddelik aksiyon listesi ile ver.
            ";

        // Google Gemini API'ye istek gönderme
        var client = _httpClientFactory.CreateClient();

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt}
                    }
                }
            }
        };

        //Json Dönüşümleri
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpResponse = await client.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}", content);

        var responseText = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            ViewBag.AIResult = $"Gemini Hatası: {httpResponse.StatusCode}";
            return View(userProfile);
        }

        //Json Yapı İçinden Veriyi Okuma
        try
        {
            using var doc = JsonDocument.Parse(responseText);
            var aiText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            ViewBag.AIResult = aiText ?? "Boş yanıt döndü";
        }
        catch (Exception ex)
        {
            ViewBag.AIResult = $"Yanıt işlenirken hata: {ex.Message}";
        }

        return View(userProfile);
    }
}
