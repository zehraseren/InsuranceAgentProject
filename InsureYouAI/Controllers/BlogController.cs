using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class BlogController : Controller
{
    private readonly InsureContext _context;

    public BlogController(InsureContext context)
    {
        _context = context;
    }

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

    public IActionResult BlogDetail(int id)
    {
        return View(id);
    }
}
