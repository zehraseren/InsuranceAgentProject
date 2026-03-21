using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.ContactDtos;

namespace InsureYouAI.Configuration.Mapping;

public class ContactMapping : Profile
{
    public ContactMapping()
    {
        // Read: DB → View
        CreateMap<Contact, ResultContactDto>();
        // Create: Form → DB
        CreateMap<CreateContactDto, Contact>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Contact, UpdateContactDto>().ReverseMap();
    }
}
