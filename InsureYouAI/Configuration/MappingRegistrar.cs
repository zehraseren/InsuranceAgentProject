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
        });

        return services;
    }
}
