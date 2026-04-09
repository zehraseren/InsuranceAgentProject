using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsureYouAI.Models.BlogDetailViewModels;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents;

public class _BDCommentListComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BDCommentListComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int articleId)
    {
        var approvedStatus = "Yorum Onaylandı";
        var commentCount = _context.Comments.Count(x => x.ArticleId == articleId && x.CommentStatus == approvedStatus);

        var comments = _context.Comments
            .Include(a => a.AppUser)
            .Where(c => c.ArticleId == articleId && c.CommentStatus == approvedStatus)
            .Select(c => new BDCommentViewModel
            {
                Author = c.AppUser.Name + " " + c.AppUser.Surname,
                CommentDate = c.CommentDate,
                CommentDetail = c.CommentDetail,
                AuthorImageUrl = c.AppUser.ImageUrl,
                CommentCount = commentCount
            }).ToList();

        return View(comments);
    }
}
