using System.Text;
using AutoMapper;
using System.Text.Json;
using InsureYouAI.Models;
using InsureYouAI.Context;
using InsureYouAI.Helpers;
using InsureYouAI.Entities;
using InsureYouAI.Attributes;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.PricingPlanDtos;

namespace InsureYouAI.Controllers;

public class PricingPlanController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public PricingPlanController(InsureContext context, IMapper mapper, HttpClient httpClient)
    {
        _context = context;
        _mapper = mapper;
        _httpClient = httpClient;
    }

    [PageInfo("Ödeme Planı", "Ödeme Planı Listesi")]
    public IActionResult PricingPlanList()
    {
        var pricingPlans = _context.PricingPlans.ToList();
        var result = _mapper.Map<List<ResultPricingPlanDto>>(pricingPlans);
        return View(result);
    }

    [HttpGet]
    [PageInfo("Ödeme Planı", "Ödeme Planı Oluştur")]
    public IActionResult CreatePricingPlan()
    {
        return View();
    }

    [HttpPost]
    [PageInfo("Ödeme Planı", "Ödeme Planı Oluştur")]
    public IActionResult CreatePricingPlan(CreatePricingPlanDto cppdto)
    {
        var pricingPlan = _mapper.Map<PricingPlan>(cppdto);
        _context.PricingPlans.Add(pricingPlan);
        _context.SaveChanges();
        return RedirectToAction("PricingPlanList");
    }

    public IActionResult DeletePricingPlan(int id)
    {
        var pricingPlan = _context.PricingPlans.Find(id);
        _context.PricingPlans.Remove(pricingPlan);
        _context.SaveChanges();
        return RedirectToAction("PricingPlanList");
    }

    [HttpGet]
    [PageInfo("Ödeme Planı", "Ödeme Planı Güncelle")]
    public IActionResult UpdatePricingPlan(int id)
    {
        var pricingPlan = _context.PricingPlans.Find(id);
        var result = _mapper.Map<UpdatePricingPlanDto>(pricingPlan);
        return View(result);
    }

    [HttpPost]
    [PageInfo("Ödeme Planı", "Ödeme Planı Güncelle")]
    public IActionResult UpdatePricingPlan(UpdatePricingPlanDto uppdto)
    {
        var pricingPlan = _mapper.Map<PricingPlan>(uppdto);
        _context.PricingPlans.Update(pricingPlan);
        _context.SaveChanges();
        return RedirectToAction("PricingPlanList");
    }

    [HttpGet]
    [PageInfo("Ödeme Planı", "AI Destekli Sigorta Önerisi")]
    public IActionResult CreateUserCustomizePlan()
    {
        return View(new AIInsuranceRecommendationViewModel());
    }

    [HttpPost]
    [PageInfo("Ödeme Planı", "AI Destekli Sigorta Önerisi")]
    public async Task<IActionResult> CreateUserCustomizePlan(AIInsuranceRecommendationViewModel aiirvm)
    {
        var userDescription = $@"
Yaş: {aiirvm.Age}
Meslek: {aiirvm.Occupation}
Şehir: {aiirvm.City}
Medeni Durum: {aiirvm.MaritalStatus.GetDisplayName()}
Çocuk Sayısı: {aiirvm.ChildrenCount}
Seyahat Sıklığı: {aiirvm.TravelFrequency.GetDisplayName()}
Aylık Bütçe: {aiirvm.MonthlyBudget} TL
Kronik Hastalık: {(aiirvm.HasChronicDisease ? "Var - " + aiirvm.ChronicDiseaseDetails : "Yok")}
Teminat Önceliği: {aiirvm.InsuranceInterest.GetDisplayName()}";

        var prompt = $@"Sen profesyonel bir sigorta uzmanı AI asistanısın.
Aşağıdaki kullanıcının bilgilerini analiz ederek en uygun sigorta paketini öner.

Paketler ve özellikleri:
1) Premium Paket (599 TL/ay): Yatarak tedavi, check-up, geniş yol yardım, yurtiçi seyahat güvencesi.
2) Standart Paket (449 TL/ay): Acil sağlık, müşteri hizmetleri, kaza sonrası tıbbi destek.
3) Ekonomik Paket (339 TL/ay): Temel sağlık, temel yol yardım.

Kullanıcı bilgileri:
{userDescription}

Sadece şu formatta JSON döndür, başka açıklama ekleme:
{{
  ""onerilenPaket"": ""Premium | Standart | Ekonomik"",
  ""ikinciSecenek"": ""Premium | Standart | Ekonomik"",
  ""neden"": ""Kısa analiz metni""
}}";

        var requestBody = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "Sadece geçerli JSON döndür. Başka açıklama, markdown veya code block ekleme."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.3
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_API_KEY_HERE");

        var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(jsonResponse);

        var aiResult = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var result = JsonSerializer.Deserialize<AIInsuranceRecommendationViewModel>(aiResult);

        aiirvm.RecommendedPackage = result?.RecommendedPackage;
        aiirvm.SecondBestPackage = result?.SecondBestPackage;
        aiirvm.AnalysisText = result?.AnalysisText;

        return View(aiirvm);
    }
}
