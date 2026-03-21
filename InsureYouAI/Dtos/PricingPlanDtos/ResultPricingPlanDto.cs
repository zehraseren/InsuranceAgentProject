namespace InsureYouAI.Dtos.PricingPlanDtos;

public class ResultPricingPlanDto
{
    public int PricingPlanId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public bool IsFeature { get; set; }
}
