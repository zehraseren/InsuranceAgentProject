using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.BlogListViewModels;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLTopArticlesComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BLTopArticlesComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var topArticles = _context.Articles
            .OrderByDescending(a => a.CreatedTime)
            .Take(3)
            .Select(a => new BLTopArticleViewModel
            {
                ArticleId = a.ArticleId,
                Title = a.Title,
                CreatedTime = a.CreatedTime
            }).ToList();

        return View(topArticles);
    }
}