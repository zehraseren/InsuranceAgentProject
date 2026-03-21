using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
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

    public IActionResult PricingPlanList()
    {
        var pricingPlans = _context.PricingPlans.ToList();
        var result = _mapper.Map<List<ResultPricingPlanDto>>(pricingPlans);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreatePricingPlan()
    {
        return View();
    }

    [HttpPost]
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
    public IActionResult UpdatePricingPlan(int id)
    {
        var pricingPlan = _context.PricingPlans.Find(id);
        var result = _mapper.Map<UpdatePricingPlanDto>(pricingPlan);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdatePricingPlan(UpdatePricingPlanDto uppdto)
    {
        var pricingPlan = _mapper.Map<PricingPlan>(uppdto);
        _context.PricingPlans.Update(pricingPlan);
        _context.SaveChanges();
        return RedirectToAction("PricingPlanList");
    }
}
