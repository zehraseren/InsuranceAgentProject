using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSubChart3ComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHSubChart3ComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var currentYear = DateTime.Now.Year - 1;
        var currentMonth = DateTime.Now.Month;

        // Geçerli yılın aylık giderlerini hesapla
        var monthlyExpenses = await _context.Expenses
            .Where(p => p.ProcessDate.Month == currentMonth && p.ProcessDate.Year == currentYear)
            .GroupBy(p => p.Detail)
            .Select(g => new DSHMonthlyExpenseViewModel
            {
                CategoryName = g.Key,
                TotalAmount = g.Sum(p => p.Amount)
            }).ToListAsync();

        return View(monthlyExpenses);
    }
}
