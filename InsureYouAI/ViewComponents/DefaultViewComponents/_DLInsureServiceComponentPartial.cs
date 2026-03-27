using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLInsureServiceComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}