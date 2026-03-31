using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLTestimonialComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLTestimonialComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var testimonials = _context.Testimonials
            .Select(t => new DLTestimonialViewModel
            {
                NameSurname = t.NameSurname,
                Title = t.Title,
                CommentDetail = t.CommentDetail,
                ImageUrl = t.ImageUrl,
            }).ToList();

        return View(testimonials);
    }
}
