using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.CommentDtos;

namespace InsureYouAI.Configuration.Mapping;

public class CommentMapping : Profile
{
    public CommentMapping()
    {
        // Read: DB → View
        CreateMap<Comment, ResultCommentDto>()
            .ForMember(a => a.ArticleTitle,
                opt => opt.MapFrom(src => src.Article.Title))
            .ForMember(au => au.NameSurname,
                opt => opt.MapFrom(src => src.AppUser.Name + " " + src.AppUser.Surname));
    }
}
