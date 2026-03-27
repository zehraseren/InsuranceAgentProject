using InsureYouAI.Dtos.RegisterUserDtos;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InsureYouAI.Controllers;

public class RegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateRegisterUserDto crudto)
    {
        if (!ModelState.IsValid)
        {
            return View(crudto);
        }

        AppUser appUser = new AppUser
        {
            Name = crudto.Name,
            Surname = crudto.Surname,
            UserName = crudto.Username,
            Email = crudto.Email,
            ImageUrl = "Url",
            Description = "Açıklama"
        };

        var result = await _userManager.CreateAsync(appUser, crudto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(crudto);
        }

        return RedirectToAction("UserList");
    }
}
