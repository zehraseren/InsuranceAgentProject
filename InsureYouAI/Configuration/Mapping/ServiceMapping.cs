using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.ServiceDtos;

namespace InsureYouAI.Configuration.Mapping;

public class ServiceMapping : Profile
{
    public ServiceMapping()
    {
        // Read: DB → View
        CreateMap<Service, ResultServiceDto>();
        // Create: Form → DB
        CreateMap<CreateServiceDto, Service>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Service, UpdateServiceDto>().ReverseMap();
    }
}
