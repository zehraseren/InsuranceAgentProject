using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLCounterComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLCounterComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var model = new DLCounterViewModel
        {
            CategoryCount = _context.Categories.Count(),
            ServiceCount = _context.Services.Count(),
            UserCount = _context.Users.Count(),
            ArticleCount = _context.Articles.Count()
        };

        return View(model);
    }
}