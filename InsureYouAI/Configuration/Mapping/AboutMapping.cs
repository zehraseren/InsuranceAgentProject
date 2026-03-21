using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.AboutDtos;

namespace InsureYouAI.Configuration.Mapping;

public class AboutMapping : Profile
{
    public AboutMapping()
    {
        // Read: DB → View
        CreateMap<About, ResultAboutDto>();
        // Create: Form → DB
        CreateMap<CreateAboutDto, About>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<About, UpdateAboutDto>().ReverseMap();
    }
}
