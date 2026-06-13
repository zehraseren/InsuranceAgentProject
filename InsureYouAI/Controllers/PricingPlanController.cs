using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.PricingPlanDtos;

namespace InsureYouAI.Controllers;

public class PricingPlanController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public PricingPlanController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
}
