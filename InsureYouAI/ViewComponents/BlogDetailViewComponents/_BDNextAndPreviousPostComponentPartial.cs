using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.BlogDetailViewModels;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents;

public class _BDNextAndPreviousPostComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _BDNextAndPreviousPostComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(int id)
    {
        var article = _context.Articles.FirstOrDefault(x => x.ArticleId == id);

        var previous = _context.Articles
            .Where(p => p.ArticleId < id)
            .OrderByDescending(p => p.ArticleId)
            .Select(p => new { p.ArticleId, p.Title })
            .FirstOrDefault();

        var next = _context.Articles
            .Where(n => n.ArticleId > id)
            .OrderBy(n => n.ArticleId)
            .Select(n => new { n.ArticleId, n.Title })
            .FirstOrDefault();

        var model = new BDPagingViewModel
        {
            CurrentId = id,
            PreviousId = previous?.ArticleId,
            PreviousTitle = previous?.Title,
            NextId = next?.ArticleId,
            NextTitle = next?.Title
        };

        return View(model);
    }
}
