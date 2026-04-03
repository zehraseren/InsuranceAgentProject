using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLTagsComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}