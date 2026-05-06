using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSubChart2ComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHSubChart2ComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currentYear = DateTime.Now.Year - 1;

        var monthlyRevenues = await _context.Policies
            .Where(p => p.StartDate.Year == currentYear)
            .GroupBy(p => p.StartDate.Month)
            .Select(g => new DSHMonthlyRevenueViewModel
            {
                Month = g.Key,
                TotalPremium = g.Sum(p => p.PremiumAmount)
            }).ToListAsync();

        // 12 Aylık Sabit Dizi (Boş aylar 0 gösterilir)
        decimal[] revenues = new decimal[12];
        foreach (var item in monthlyRevenues)
        {
            revenues[item.Month - 1] = item.TotalPremium;
        }

        return View(revenues);
    }
}
