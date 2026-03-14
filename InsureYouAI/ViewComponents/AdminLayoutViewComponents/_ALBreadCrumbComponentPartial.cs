using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.AdminLayoutViewComponents;

public class _ALBreadCrumbComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
