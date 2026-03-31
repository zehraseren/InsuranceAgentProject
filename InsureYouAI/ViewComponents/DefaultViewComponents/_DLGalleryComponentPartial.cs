using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLGalleryComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLGalleryComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var galleries = _context.Galleries
            .Select(g => new DLGalleryViewModel
            {
                Title = g.Title,
                ImageUrl = g.ImageUrl
            }).ToList();

        return View(galleries);
    }
}