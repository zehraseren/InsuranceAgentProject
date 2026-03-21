using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.ArticleDtos;

namespace InsureYouAI.Configuration.Mapping;

public class ArticleMapping : Profile
{
    public ArticleMapping()
    {
        // Read: DB → View
        CreateMap<Article, ResultArticleDto>();
        // Create: Form → DB
        CreateMap<CreateArticleDto, Article>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Article, UpdateArticleDto>().ReverseMap();
    }
}
