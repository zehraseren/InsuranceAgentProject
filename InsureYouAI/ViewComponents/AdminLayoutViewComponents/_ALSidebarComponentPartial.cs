using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.AdminLayoutViewComponents;

public class _ALSidebarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
