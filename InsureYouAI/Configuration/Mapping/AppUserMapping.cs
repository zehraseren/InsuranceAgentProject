using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.AppUserDtos;

namespace InsureYouAI.Configuration.Mapping;

public class AppUserMapping : Profile
{
    public AppUserMapping()
    {
        // Read: DB → View
        CreateMap<AppUser, ResultAppUserDto>()
            .ForMember(a => a.Author,
                opt => opt.MapFrom(src => src.Name + " " + src.Surname));
    }
}
