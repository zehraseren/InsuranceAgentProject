using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLServiceComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLServiceComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var services = _context.Services
            .Select(s => new DLServiceViewModel
            {
                Title = s.Title,
                Description = s.Description,
                IconUrl = s.IconUrl,
                ImageUrl = s.ImageUrl
            }).ToList();
        return View(services);
    }
}