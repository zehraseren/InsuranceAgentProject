using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLAboutComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLAboutComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var aboutInfo = _context.Abouts.FirstOrDefault();
        var model = new DLAboutViewModel
        {
            Title = aboutInfo.Title,
            Description = aboutInfo.Description,
            ImageUrl = aboutInfo.ImageUrl,
            Details = _context.AboutItems.Select(ai => ai.Detail).ToList()
        };

        return View(model);
    }
}