using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.SliderDtos;

namespace InsureYouAI.Configuration.Mapping;

public class SliderMapping : Profile
{
    public SliderMapping()
    {
        // Read: DB → View
        CreateMap<Slider, ResultSliderDto>();
        // Create: Form → DB
        CreateMap<CreateSliderDto, Slider>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Slider, UpdateSliderDto>().ReverseMap();
    }
}
