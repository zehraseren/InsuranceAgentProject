using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.BlogListViewModels;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLSearchArticlesByCategoryComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BLSearchArticlesByCategoryComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int id)
    {
        var articles = _context.Articles
            .Where(a => a.CategoryId == id)
            .Select(a => new BLBlogListViewModel
            {
                ArticleId = a.ArticleId,
                Title = a.Title,
                CreatedDate = a.CreatedTime,
                Content = a.Content,
                CoverImageUrl = a.CoverImageUrl,
                MainCoverImageUrl = a.MainCoverImageUrl,
                CategoryName = a.Category.CategoryName,
                Author = a.AppUser.Name + " " + a.AppUser.Surname,
                CommentCount = a.Comments.Count
            }).ToList();

        return View(articles);
    }
}
