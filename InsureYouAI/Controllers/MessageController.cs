using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.MessageDtos;

namespace InsureYouAI.Controllers;

public class MessageController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public MessageController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult MessageList()
    {
        var messages = _context.Messages.ToList();
        var result = _mapper.Map<List<ResultMessageDto>>(messages);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateMessage()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateMessage(CreateMessageDto cmdto)
    {
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
    public IActionResult UpdateMessage(int id)
    {
        var message = _context.Messages.Find(id);
        var result = _mapper.Map<UpdateMessageDto>(message);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateMessage(UpdateMessageDto umdto)
    {
        var message = _mapper.Map<Message>(umdto);
        _context.Messages.Update(message);
        _context.SaveChanges();
        return RedirectToAction("MessageList");
    }
}
