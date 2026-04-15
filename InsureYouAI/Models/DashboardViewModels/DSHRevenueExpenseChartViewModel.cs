namespace InsureYouAI.Models.DashboardViewModels;

public class DSHRevenueExpenseChartViewModel
{
    public List<string> Months { get; set; }
    public List<decimal> Revenues { get; set; }
    public List<decimal> Expenses { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpense { get; set; }
}
