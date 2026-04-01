using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLLast3ArticleComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLLast3ArticleComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var articles = _context.Articles
            .Include(a => a.Category)
            .OrderByDescending(a => a.ArticleId)
            .Take(3)
            .Select(a => new DLArticleViewModel
            {
                Title = a.Title,
                CreatedTime = a.CreatedTime,
                Content = a.Content,
                CoverImageUrl = a.CoverImageUrl,
                MainCoverImageUrl = a.MainCoverImageUrl,
                CategoryName = a.Category.CategoryName,
                Comments = a.Comments.ToList()
            })
            .ToList();

        return View(articles);
    }
}