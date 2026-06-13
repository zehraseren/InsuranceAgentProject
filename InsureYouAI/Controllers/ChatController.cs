using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class ChatController : Controller
{
    [PageInfo("Sohbet", "AI ile Sohbet")]
    public IActionResult ChatWithAI()
    {
        return View();
    }
}
