using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHUserQuickViewComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHUserQuickViewComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var values = _context.Users
            .GroupJoin(
                _context.Policies,
                user => user.Id,
                policy => policy.AppUserId,
                (user, policies) => new DSHUserQuickViewModel
                {
                    UserId = user.Id,
                    FullName = user.Name + " " + user.Surname,
                    ImageUrl = user.ImageUrl,
                    PolicyCount = policies.Count(),
                    TotalPremium = policies.Sum(p => (decimal?)p.PremiumAmount) ?? 0
                })
            .OrderByDescending(x => x.PolicyCount)
            .ToList();

        return View(values);
    }
}