using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLFooterComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLFooterComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var contact = _context.Contacts
            .Select(c => new DLFooterViewModel
            {
                Description = c.Description,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address
            }).FirstOrDefault();

        return View(contact);
    }
}