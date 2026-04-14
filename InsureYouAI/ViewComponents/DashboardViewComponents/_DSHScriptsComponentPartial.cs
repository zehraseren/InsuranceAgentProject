using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DashboardViewComponents;

public class _DSHScriptsComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}