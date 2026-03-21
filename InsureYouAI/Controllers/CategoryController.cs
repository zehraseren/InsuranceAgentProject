using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.CategoryDtos;

namespace InsureYouAI.Controllers;

public class CategoryController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public CategoryController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult CategoryList()
    {
        var categories = _context.Categories.ToList();
        var result = _mapper.Map<List<ResultCategoryDto>>(categories);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateCategory(CreateCategoryDto ccdto)
    {
        var category = _mapper.Map<Category>(ccdto);
        _context.Categories.Add(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }

    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        _context.Categories.Remove(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }

    [HttpGet]
    public IActionResult UpdateCategory(int id)
    {
        var category = _context.Categories.Find(id);
        var result = _mapper.Map<UpdateCategoryDto>(category);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateCategory(UpdateCategoryDto ucdto)
    {
        var category = _mapper.Map<Category>(ucdto);
        _context.Categories.Update(category);
        _context.SaveChanges();
        return RedirectToAction("CategoryList");
    }
}
