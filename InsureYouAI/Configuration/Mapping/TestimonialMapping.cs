using AutoMapper;
using InsureYouAI.Entities;
using InsureYouAI.Dtos.TestimonialDtos;

namespace InsureYouAI.Configuration.Mapping;

public class TestimonialMapping : Profile
{
    public TestimonialMapping()
    {
        // Read: DB → View
        CreateMap<Testimonial, ResultTestimonialDto>();
        // Create: Form → DB
        CreateMap<CreateTestimonialDto, Testimonial>();
        // Update: DB → Form (HttpGet) ve Form → DB (HttpPost) | ReverseMap ile iki yönlü
        CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();
    }
}
