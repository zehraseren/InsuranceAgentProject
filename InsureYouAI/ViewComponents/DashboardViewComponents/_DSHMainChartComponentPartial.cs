using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHMainChartComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHMainChartComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        // Revenue
        var revenueData = _context.Revenues
            .GroupBy(r => r.ProcessDate.Month)
            .Select(g => new
            {
                Month = g.Key,
                TotalAmount = g.Sum(x => x.Amount)
            })
            .OrderBy(x => x.Month)
            .ToList();

        // Expense
        var expenseData = _context.Expenses
            .GroupBy(r => r.ProcessDate.Month)
            .Select(g => new
            {
                Month = g.Key,
                TotalAmount = g.Sum(x => x.Amount)
            })
            .OrderBy(x => x.Month)
            .ToList();

        // All Months
        var allMonths = revenueData.Select(x => x.Month)
            .Union(expenseData.Select(y => y.Month))
            .OrderBy(x => x)
            .ToList();

        var model = new DSHRevenueExpenseChartViewModel
        {
            Months = allMonths.Select(x => new System.Globalization.DateTimeFormatInfo().GetAbbreviatedMonthName(x)).ToList(),
            Revenues = allMonths.Select(m => revenueData.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0).ToList(),
            Expenses = allMonths.Select(m => expenseData.FirstOrDefault(e => e.Month == m)?.TotalAmount ?? 0).ToList(),
            TotalRevenue = _context.Revenues.Sum(r => r.Amount),
            TotalExpense = _context.Expenses.Sum(e => e.Amount)
        };

        return View(model);
    }
}