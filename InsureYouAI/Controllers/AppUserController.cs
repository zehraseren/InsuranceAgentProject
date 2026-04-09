using AutoMapper;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.AppUserDtos;
using Microsoft.AspNetCore.Identity;
using AutoMapper.QueryableExtensions;

namespace InsureYouAI.Controllers;

public class AppUserController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public AppUserController(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public IActionResult UserList()
    {
        var users = _userManager.Users.ProjectTo<ResultAppUserDto>(_mapper.ConfigurationProvider).ToList();

        return View(users);
    }
}
