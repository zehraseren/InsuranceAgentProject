using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHPolicyTypesComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHPolicyTypesComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var groups = _context.Policies
            .GroupBy(p => p.PolicyType)
            .Select(g => new DSHPolicyGroupViewModel
            {
                PolicyType = g.Key,
                Count = g.Count(),
            }).ToList();

        var total = groups.Sum(g => g.Count);

        foreach (var item in groups)
        {
            item.Percentage = total == 0 ? 0 : (item.Count * 100 / total);
        }

        return View(groups);
    }
}