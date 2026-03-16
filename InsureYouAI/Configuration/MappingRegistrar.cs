using AutoMapper;
using InsureYouAI.Configuration.Mapping;

namespace InsureYouAI.Configuration;

public static class MappingRegistrar
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
        });

        return services;
    }
}
