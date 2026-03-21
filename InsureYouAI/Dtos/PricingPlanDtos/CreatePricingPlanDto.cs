namespace InsureYouAI.Dtos.PricingPlanDtos;

public class CreatePricingPlanDto
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public bool IsFeature { get; set; }
}
