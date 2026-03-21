using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.SliderDtos;

namespace InsureYouAI.Controllers;

public class SliderController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public SliderController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult SliderList()
    {
        var sliders = _context.Sliders.ToList();
        var result = _mapper.Map<List<ResultSliderDto>>(sliders);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateSlider()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateSlider(CreateSliderDto csdto)
    {
        var slider = _mapper.Map<Slider>(csdto);
        _context.Sliders.Add(slider);
        _context.SaveChanges();
        return RedirectToAction("SliderList");
    }

    public IActionResult DeleteSlider(int id)
    {
        var slider = _context.Sliders.Find(id);
        _context.Sliders.Remove(slider);
        _context.SaveChanges();
        return RedirectToAction("SliderList");
    }

    [HttpGet]
    public IActionResult UpdateSlider(int id)
    {
        var slider = _context.Sliders.Find(id);
        var result = _mapper.Map<UpdateSliderDto>(slider);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateSlider(UpdateSliderDto usdto)
    {
        var slider = _mapper.Map<Slider>(usdto);
        _context.Sliders.Update(slider);
        _context.SaveChanges();
        return RedirectToAction("SliderList");
    }
}
