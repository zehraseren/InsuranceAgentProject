using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLFooterLast2ArticleComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLFooterLast2ArticleComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var articles = _context.Articles.
            Select(a => new DLFooterLast2ArticleViewModel
            {
                Title = a.Title,
                CoverImageUrl = a.CoverImageUrl,
                CreatedDate = a.CreatedTime
            }).Skip(3).Take(2).ToList();

        return View(articles);
    }
}