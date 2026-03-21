using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ServiceDtos;

namespace InsureYouAI.Controllers;

public class ServiceController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public ServiceController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult ServiceList()
    {
        var services = _context.Services.ToList();
        var result = _mapper.Map<List<ResultServiceDto>>(services);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateService()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateService(CreateServiceDto csdto)
    {
        var service = _mapper.Map<Service>(csdto);
        _context.Services.Add(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }

    public IActionResult DeleteService(int id)
    {
        var service = _context.Services.Find(id);
        _context.Services.Remove(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }

    [HttpGet]
    public IActionResult UpdateService(int id)
    {
        var service = _context.Services.Find(id);
        var result = _mapper.Map<UpdateServiceDto>(service);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateService(UpdateServiceDto usdto)
    {
        var service = _mapper.Map<Service>(usdto);
        _context.Services.Update(service);
        _context.SaveChanges();
        return RedirectToAction("ServiceList");
    }
}
