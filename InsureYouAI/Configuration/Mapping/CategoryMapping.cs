using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.CategoryDtos;

namespace InsureYouAI.Configuration.Mapping;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        // Read: DB → View
        CreateMap<Category, ResultCategoryDto>();
        // Create: Form → DB
        CreateMap<CreateCategoryDto, Category>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();
    }
}
