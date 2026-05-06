using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSubChartsComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
