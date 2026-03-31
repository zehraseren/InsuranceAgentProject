using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLPricingPlanComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLPricingPlanComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var pricingPlans = _context.PricingPlans
            .Where(pp => pp.IsFeature)
            .Select(pp=> new DLPricingPlanViewModel
            {
                Title = pp.Title,
                Price = pp.Price,
                IsFeature = pp.IsFeature,
                PricingPlanItems = pp.PricingPlanItems.ToList(),
            }).ToList();

        return View(pricingPlans);
    }
}