using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.AboutItemDtos;

namespace InsureYouAI.Controllers;

public class AboutItemController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public AboutItemController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult AboutItemList()
    {
        var aboutItems = _context.AboutItems.ToList();
        var result = _mapper.Map<List<ResultAboutItemDto>>(aboutItems);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateAboutItem()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAboutItem(CreateAboutItemDto caidto)
    {
        var aboutItem = _mapper.Map<AboutItem>(caidto);
        _context.AboutItems.Add(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }

    public IActionResult DeleteAboutItem(int id)
    {
        var aboutItem = _context.AboutItems.Find(id);
        _context.AboutItems.Remove(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }

    [HttpGet]
    public IActionResult UpdateAboutItem(int id)
    {
        var aboutItem = _context.AboutItems.Find(id);
        var result = _mapper.Map<UpdateAboutItemDto>(aboutItem);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateAboutItem(UpdateAboutItemDto uaidto)
    {
        var aboutItem = _mapper.Map<AboutItem>(uaidto);
        _context.AboutItems.Update(aboutItem);
        _context.SaveChanges();
        return RedirectToAction("AboutItemList");
    }
}
