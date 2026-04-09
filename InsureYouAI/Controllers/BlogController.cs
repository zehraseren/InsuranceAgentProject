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

    [HttpGet]
    public PartialViewResult AddComment()
    {
        return PartialView();
    }

    [HttpPost]
    public IActionResult AddComment(Comment comment)
    {
        comment.CommentDate = DateTime.Now;
        comment.AppUserId = "8dcfb6c9-9620-40d9-8060-1e702870d001";
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return RedirectToAction("BlogList");
    }
}
