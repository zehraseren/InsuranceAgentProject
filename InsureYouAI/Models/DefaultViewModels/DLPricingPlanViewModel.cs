using InsureYouAI.Entities;

namespace InsureYouAI.Models.DefaultViewModels;

public class DLPricingPlanViewModel
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public bool IsFeature { get; set; }
    public List<PricingPlanItem> PricingPlanItems { get; set; }
}
