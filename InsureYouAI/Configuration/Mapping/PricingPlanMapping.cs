using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.PricingPlanDtos;

namespace InsureYouAI.Configuration.Mapping;

public class PricingPlanMapping : Profile
{
    public PricingPlanMapping()
    {
        // Read: DB → View
        CreateMap<PricingPlan, ResultPricingPlanDto>();
        // Create: Form → DB
        CreateMap<CreatePricingPlanDto, PricingPlan>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<PricingPlan, UpdatePricingPlanDto>().ReverseMap();
    }
}
