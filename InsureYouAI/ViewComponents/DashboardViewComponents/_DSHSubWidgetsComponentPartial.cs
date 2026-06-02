using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSubWidgetsComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHSubWidgetsComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var currentDate = DateTime.Now;
        var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        var model = new DSHSubWidgetsViewModel
        {
            TotalCategoryCount = _context.Categories.Count(),
            TotalArticleCount = _context.Articles.Count(),
            TotalPoliciesCount = _context.Policies.Count(),
            TotalPoliciesByThisMonthCount = _context.Policies
                .Where(p => p.CreatedDate >= startOfMonth && p.CreatedDate < startOfNextMonth)
                .Count(),
            TotalCommentCount = _context.Comments.Count(),
            TotalUserCount = _context.Users.Count(),
            AvgPolicyAmount = _context.Policies.Average(a => a.PremiumAmount),
            LastRevenueAmount = _context.Revenues
                .OrderByDescending(r => r.RevenueId)
                .Select(r => r.Amount)
                .FirstOrDefault()
        };

        return View(model);
    }
}