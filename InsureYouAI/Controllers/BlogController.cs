using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class BlogController : Controller
{
    public IActionResult BlogList()
    {
        return View();
    }

    public PartialViewResult SearchBlogs()
    {
        return PartialView();
    }
    [HttpPost]
    public IActionResult SearchBlogs(string keyword)
    {
        return PartialView();
    }

    public IActionResult BlogDetail()
    {
        return View();
    }
}
