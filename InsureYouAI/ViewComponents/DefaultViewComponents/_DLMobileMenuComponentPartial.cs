using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Models.DefaultViewModels;

namespace InsureYouAI.ViewComponents.DefaultViewComponents;

public class _DLMobileMenuComponentPartial : ViewComponent
{
    private readonly InsureContext _context;

    public _DLMobileMenuComponentPartial(InsureContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var contact = _context.Contacts
            .Select(c => new DLHeaderContactViewModel
            {
                Email = c.Email,
                Phone = c.Phone
            }).FirstOrDefault();

        return View(contact ?? new DLHeaderContactViewModel());
    }
}