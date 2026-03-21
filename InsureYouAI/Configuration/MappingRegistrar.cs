using InsureYouAI.Configuration.Mapping;

namespace InsureYouAI.Configuration;

public static class MappingRegistrar
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CategoryMapping>();
            cfg.AddProfile<AboutMapping>();
            cfg.AddProfile<AboutItemMapping>();
            cfg.AddProfile<ContactMapping>();
            cfg.AddProfile<MessageMapping>();
            cfg.AddProfile<PricingPlanMapping>();
            cfg.AddProfile<ServiceMapping>();
            cfg.AddProfile<SliderMapping>();
        });

        return services;
    }
}
