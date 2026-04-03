using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class BlogController : Controller
{
    public IActionResult BlogList()
    {
        return View();
    }
}
