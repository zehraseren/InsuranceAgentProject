using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;

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
        var about = _context.Abouts.FirstOrDefault();
        var model = new Models.DefaultViewModels.DLAboutViewModel
        {
            Title = about.Title,
            Description = about.Description,
            ImageUrl = about.ImageUrl,
            Details = _context.AboutItems.Select(ai => ai.Detail).ToList()
        };

        return View(model);
    }
}