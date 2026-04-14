using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.GroqAIMessageDtos;

namespace InsureYouAI.Configuration.Mapping;

public class GroqAIMessageMapping : Profile
{
    public GroqAIMessageMapping()
    {
        CreateMap<GroqAIMessage, ResultGroqAIMessageDto>().ReverseMap();
    }
}
