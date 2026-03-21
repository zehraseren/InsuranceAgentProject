using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.MessageDtos;

namespace InsureYouAI.Configuration.Mapping;

public class MessageMapping : Profile
{
    public MessageMapping()
    {
        // Read: DB → View
        CreateMap<Message, ResultMessageDto>();
        // Create: Form → DB
        CreateMap<CreateMessageDto, Message>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Message, UpdateMessageDto>().ReverseMap();
    }
}
