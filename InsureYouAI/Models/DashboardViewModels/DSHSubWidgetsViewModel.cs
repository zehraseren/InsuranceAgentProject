namespace InsureYouAI.Models.DashboardViewModels;

public class DSHSubWidgetsViewModel
{
    public int TotalCategoryCount { get; set; }
    public int TotalArticleCount { get; set; }
    public int TotalPoliciesCount { get; set; }
    public int TotalPoliciesByThisMonthCount { get; set; }
    public int TotalCommentCount { get; set; }
    public int TotalUserCount { get; set; }
    public decimal AvgPolicyAmount { get; set; }
    public decimal LastRevenueAmount { get; set; }
}
