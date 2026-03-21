using AutoMapper;
using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.ContactDtos;

namespace InsureYouAI.Controllers;

public class ContactController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public ContactController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult ContactList()
    {
        var contacts = _context.Contacts.ToList();
        var result = _mapper.Map<List<ResultContactDto>>(contacts);
        return View(result);
    }

    [HttpGet]
    public IActionResult CreateContact()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateContact(CreateContactDto ccidto)
    {
        var contact = _mapper.Map<Contact>(ccidto);
        _context.Contacts.Add(contact);
        _context.SaveChanges();
        return RedirectToAction("ContactList");
    }

    public IActionResult DeleteContact(int id)
    {
        var contact = _context.Contacts.Find(id);
        _context.Contacts.Remove(contact);
        _context.SaveChanges();
        return RedirectToAction("ContactList");
    }

    [HttpGet]
    public IActionResult UpdateContact(int id)
    {
        var contact = _context.Contacts.Find(id);
        var result = _mapper.Map<UpdateContactDto>(contact);
        return View(result);
    }

    [HttpPost]
    public IActionResult UpdateContact(UpdateContactDto ucidto)
    {
        var contact = _mapper.Map<Contact>(ucidto);
        _context.Contacts.Update(contact);
        _context.SaveChanges();
        return RedirectToAction("ContactList");
    }
}