using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.AboutItemDtos;

namespace InsureYouAI.Configuration.Mapping;

public class AboutItemMapping : Profile
{
    public AboutItemMapping()
    {
        // Read: DB → View
        CreateMap<AboutItem, ResultAboutItemDto>();
        // Create: Form → DB
        CreateMap<CreateAboutItemDto, AboutItem>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<AboutItem, UpdateAboutItemDto>().ReverseMap();
    }
}
