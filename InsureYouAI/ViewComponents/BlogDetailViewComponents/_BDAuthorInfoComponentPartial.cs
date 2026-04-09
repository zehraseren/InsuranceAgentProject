using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.ViewComponents.BlogDetailViewComponents;

public class _BDAuthorInfoComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
