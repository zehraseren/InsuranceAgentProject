using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.BlogDetailViewModels;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents;

public class _BDAuthorInfoComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BDAuthorInfoComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int id)
    {
        var authorInfo = _context.Articles
            .Where(a => a.ArticleId == id)
            .Select(au => new BDAuthorInfoViewModel
            {
                NameSurname = au.AppUser.Name + " " + au.AppUser.Surname,
                Description = au.AppUser.Description,
                Title = au.AppUser.Title,
                ImageUrl = au.AppUser.ImageUrl
            }).FirstOrDefault();

        if (authorInfo == null) return Content("");

        return View(authorInfo);
    }
}
