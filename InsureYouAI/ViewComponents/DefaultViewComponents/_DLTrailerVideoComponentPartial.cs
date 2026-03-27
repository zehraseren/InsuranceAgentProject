using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLTrailerVideoComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLTrailerVideoComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var trailerVideo = _context.TrailerVideos
            .Select(tv => new DLTrailerVideoViewModel
            {
                Title = tv.Title,
                CoverImageUrl = tv.CoverImageUrl,
                VideoUrl = tv.VideoUrl,
            }).ToList();

        return View(trailerVideo);
    }
}