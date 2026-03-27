using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLSliderComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLSliderComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var sliders = _context.Sliders
            .Select(s => new DLSliderViewModel
            {
                SliderId = s.SliderId,
                Title = s.Title,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
            }).ToList();
        return View(sliders);
    }
}