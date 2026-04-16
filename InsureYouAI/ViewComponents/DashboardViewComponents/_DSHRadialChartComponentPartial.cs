using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHRadialChartComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHRadialChartComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var model = new DSHRadialChartViewModel
        {
            TotalPolicies = _context.Policies.Count(),
            ActivePolicies = _context.Policies.Count(p => p.Status == "Active")
        };

        return View(model);
    }
}
