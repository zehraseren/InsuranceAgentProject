using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.TrailerVideoDtos;

namespace InsureYouAI.Configuration.Mapping;

public class TrailerVideoMapping : Profile
{
    public TrailerVideoMapping()
    {
        // Read: DB → View
        CreateMap<TrailerVideo, ResultTrailerVideoDto>();
        // Create: Form → DB
        CreateMap<CreateTrailerVideoDto, TrailerVideo>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<TrailerVideo, UpdateTrailerVideoDto>().ReverseMap();
    }
}
