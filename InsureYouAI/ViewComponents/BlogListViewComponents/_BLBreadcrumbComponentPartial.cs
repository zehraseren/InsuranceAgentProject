using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogListViewComponents;

public class _BLBreadcrumbComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
