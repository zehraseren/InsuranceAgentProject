using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHSecondaryChartComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}