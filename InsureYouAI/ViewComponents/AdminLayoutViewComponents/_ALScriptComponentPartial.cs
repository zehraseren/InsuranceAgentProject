using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.AdminLayoutViewComponents;

public class _ALScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
