using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.BlogListViewModels;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLCategoriesComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BLCategoriesComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var categoryArticles = _context.Categories
            .Select(ca => new BLCategoryArticlesViewModel
            {
                CategoryId = ca.CategoryId,
                CategoryName = ca.CategoryName,
                ArticleCount = ca.Articles.Count()
            }).ToList();

        return View(categoryArticles);
    }
}