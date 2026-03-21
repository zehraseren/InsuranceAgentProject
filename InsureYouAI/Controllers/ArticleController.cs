using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ArticleDtos;

namespace InsureYouAI.Controllers;

public class ArticleController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public ArticleController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult ArticleList()
    {
        var articles = _context.Articles.ToList();
        var result = _mapper.Map<List<ResultArticleDto>>(articles);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateArticle()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateArticle(CreateArticleDto cadto)
    {
        cadto.CreatedTime = DateTime.Now;
        var article = _mapper.Map<Article>(cadto);
        _context.Articles.Add(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }

    public IActionResult DeleteArticle(int id)
    {
        var article = _context.Articles.Find(id);
        _context.Articles.Remove(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }

    [HttpGet]
    public IActionResult UpdateArticle(int id)
    {
        var article = _context.Articles.Find(id);
        var result = _mapper.Map<UpdateArticleDto>(article);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateArticle(UpdateArticleDto uadto)
    {
        var article = _mapper.Map<Article>(uadto);
        _context.Articles.Update(article);
        _context.SaveChanges();
        return RedirectToAction("ArticleList");
    }
}
