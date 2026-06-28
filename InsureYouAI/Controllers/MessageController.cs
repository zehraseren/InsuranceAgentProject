using AutoMapper;
using InsureYouAI.Enums;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Services;
using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.MessageDtos;

namespace InsureYouAI.Controllers;

public class MessageController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;
    private readonly AIService _aIService;

    public MessageController(InsureContext context, IMapper mapper, AIService aIService)
    {
        _context = context;
        _mapper = mapper;
        _aIService = aIService;
    }

    [PageInfo("Mesajlar", "Mesaj Listesi")]
    public IActionResult MessageList()
    {
        var messages = _context.Messages.ToList();
        var result = _mapper.Map<List<ResultMessageDto>>(messages);
        return View(result);
    }

    [HttpGet]
    [PageInfo("Mesajlar", "Mesaj Oluştur")]
    public IActionResult CreateMessage()
    {
        return View();
    }

    [HttpPost]
    [PageInfo("Mesajlar", "Mesaj Oluştur")]
    public async Task<IActionResult> CreateMessage(CreateMessageDto cmdto)
    {
        if (!ModelState.IsValid) return View(cmdto);

        var combinedText = $"{cmdto.Subject} - {cmdto.MessageDetail}";

        var result = await _aIService.AnalyzeMessageAsync(combinedText);

        cmdto.AICategory = result.Category;
        cmdto.IsRead = false;
        cmdto.SendDate = DateTime.Now;

        var message = _mapper.Map<Message>(cmdto);

        _context.Messages.Add(message);
        _context.SaveChanges();

        return RedirectToAction("MessageList");
    }

    public IActionResult DeleteMessage(int id)
    {
        var message = _context.Messages.Find(id);
        _context.Messages.Remove(message);
        _context.SaveChanges();
        return RedirectToAction("MessageList");
    }

    [HttpGet]
    [PageInfo("Mesajlar", "Mesaj Güncelle")]
    public IActionResult UpdateMessage(int id)
    {
        var message = _context.Messages.Find(id);
        var result = _mapper.Map<UpdateMessageDto>(message);
        return View(result);
    }

    [HttpPost]
    [PageInfo("Mesajlar", "Mesaj Güncelle")]
    public IActionResult UpdateMessage(UpdateMessageDto umdto)
    {
        var message = _mapper.Map<Message>(umdto);
        _context.Messages.Update(message);
        _context.SaveChanges();
        return RedirectToAction("MessageList");
    }
}
