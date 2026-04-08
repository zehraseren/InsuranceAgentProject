using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.BlogDetailViewModels;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents;

public class _BDArticleContentComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BDArticleContentComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int articleId)
    {
        var articles = _context.Articles
        .Include(ca => ca.Category)
        .Include(u => u.AppUser)
        .Include(cu => cu.Comments)
        .Where(a => a.ArticleId == articleId)
        .Select(a => new BDArticleContentViewModel
        {
            ArticleId = a.ArticleId,
            Title = a.Title,
            CreatedDate = a.CreatedTime,
            Content = a.Content,
            CategoryName = a.Category.CategoryName,
            Author = a.AppUser.Name + " " + a.AppUser.Surname,
            CommentCount = a.Comments.Count()
        }).FirstOrDefault();

        return View(articles);
    }
}