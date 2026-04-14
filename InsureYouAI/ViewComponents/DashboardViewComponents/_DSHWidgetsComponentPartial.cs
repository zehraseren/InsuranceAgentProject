using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHWidgetsComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHWidgetsComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var random = new Random();

        var model = new DSHWidgetsViewModel
        {
            ArticleCount = _context.Articles.Count(),
            CategoryCount = _context.Categories.Count(),
            CommentCount = _context.Comments.Count(),
            UserCount = _context.Users.Count(),
            Rate1 = $"{random.Next(1, 30)}.{random.Next(0, 10)}",
            Rate2 = $"{random.Next(1, 30)}.{random.Next(0, 10)}",
            Rate3 = $"{random.Next(1, 30)}.{random.Next(0, 10)}",
            Rate4 = $"{random.Next(1, 30)}.{random.Next(0, 10)}"
        };

        return View(model);
    }
}
