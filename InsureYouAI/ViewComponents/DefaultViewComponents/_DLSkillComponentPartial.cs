using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLSkillComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}