using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.AboutDtos;

namespace InsureYouAI.Controllers;

public class AboutController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public AboutController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult AboutList()
    {
        var abouts = _context.Abouts.ToList();
        var result = _mapper.Map<List<ResultAboutDto>>(abouts);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateAbout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAbout(CreateAboutDto cadto)
    {
        var about = _mapper.Map<About>(cadto);
        _context.Abouts.Add(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }

    public IActionResult DeleteAbout(int id)
    {
        var about = _context.Abouts.Find(id);
        _context.Abouts.Remove(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }

    [HttpGet]
    public IActionResult UpdateAbout(int id)
    {
        var about = _context.Abouts.Find(id);
        var result = _mapper.Map<UpdateAboutDto>(about);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateAbout(UpdateAboutDto uadto)
    {
        var about = _mapper.Map<About>(uadto);
        _context.Abouts.Update(about);
        _context.SaveChanges();
        return RedirectToAction("AboutList");
    }
}
