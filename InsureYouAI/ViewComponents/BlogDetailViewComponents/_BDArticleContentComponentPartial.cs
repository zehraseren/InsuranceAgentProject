using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
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