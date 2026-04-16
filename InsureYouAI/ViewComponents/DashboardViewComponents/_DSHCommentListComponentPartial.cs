using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DashboardViewModels;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHCommentListComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DSHCommentListComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var comments = _context.Comments
            .OrderByDescending(c => c.CommentId)
            .Take(7)
            .Select(c => new DSHCommentListViewModel
            {
                NameSurname = c.AppUser.Name + " " + c.AppUser.Surname,
                CommentDetail = c.CommentDetail,
                ImageUrl = c.AppUser.ImageUrl,
                CommentDate = c.CommentDate
            }).ToList();

        return View(comments);
    }
}