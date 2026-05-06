using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSubChart1ComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHSubChart1ComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var model = _context.Policies
           .GroupBy(p => p.PolicyType)
           .Select(g => new DSHPolicyTypeCountViewModel
           {
               PolicyType = g.Key,
               Count = g.Count()
           }).ToList();

        return View(model);
    }
}
