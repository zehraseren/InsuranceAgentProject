using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.TestimonialDtos;

namespace InsureYouAI.Controllers;

public class TestimonialController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public TestimonialController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult TestimonialList()
    {
        var testimonials = _context.Testimonials.ToList();
        var result = _mapper.Map<List<ResultTestimonialDto>>(testimonials);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateTestimonial()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTestimonial(CreateTestimonialDto ctdto)
    {
        var testimonial = _mapper.Map<Testimonial>(ctdto);
        _context.Testimonials.Add(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }

    public IActionResult DeleteTestimonial(int id)
    {
        var testimonial = _context.Testimonials.Find(id);
        _context.Testimonials.Remove(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }

    [HttpGet]
    public IActionResult UpdateTestimonial(int id)
    {
        var testimonial = _context.Testimonials.Find(id);
        var result = _mapper.Map<UpdateTestimonialDto>(testimonial);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateTestimonial(UpdateTestimonialDto utdto)
    {
        var testimonial = _mapper.Map<Testimonial>(utdto);
        _context.Testimonials.Update(testimonial);
        _context.SaveChanges();
        return RedirectToAction("TestimonialList");
    }
}
