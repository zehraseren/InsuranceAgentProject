using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.TrailerVideoDtos;

namespace InsureYouAI.Controllers;

public class TrailerVideoController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public TrailerVideoController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult TrailerVideoList()
    {
        var trailerVideos = _context.TrailerVideos.ToList();
        var result = _mapper.Map<List<ResultTrailerVideoDto>>(trailerVideos);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateTrailerVideo()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTrailerVideo(CreateTrailerVideoDto ctvdto)
    {
        var trailerVideo = _mapper.Map<TrailerVideo>(ctvdto);
        _context.TrailerVideos.Add(trailerVideo);
        _context.SaveChanges();
        return RedirectToAction("TrailerVideoList");
    }

    public IActionResult DeleteTrailerVideo(int id)
    {
        var trailerVideo = _context.TrailerVideos.Find(id);
        _context.TrailerVideos.Remove(trailerVideo);
        _context.SaveChanges();
        return RedirectToAction("TrailerVideoList");
    }

    [HttpGet]
    public IActionResult UpdateTrailerVideo(int id)
    {
        var trailerVideo = _context.TrailerVideos.Find(id);
        var result = _mapper.Map<UpdateTrailerVideoDto>(trailerVideo);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateTrailerVideo(UpdateTrailerVideoDto utvdto)
    {
        var trailerVideo = _mapper.Map<TrailerVideo>(utvdto);
        _context.TrailerVideos.Update(trailerVideo);
        _context.SaveChanges();
        return RedirectToAction("TrailerVideoList");
    }
}
