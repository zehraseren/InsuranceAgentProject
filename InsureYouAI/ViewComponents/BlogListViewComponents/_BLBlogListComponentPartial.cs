using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.BlogListViewModels;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLBlogListComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BLBlogListComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var articles = _context.Articles
             .Include(a => a.Category)
             .Include(u => u.AppUser)
             .Include(c => c.Comments)
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
                 CommentCount = a.Comments.Count()
             })
             .ToList();

        return View(articles);
    }
}