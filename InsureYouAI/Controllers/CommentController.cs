using AutoMapper;
using InsureYouAI.Context;
using Microsoft.AspNetCore.Mvc;
using InsureYouAI.Dtos.CommentDtos;
using AutoMapper.QueryableExtensions;

namespace InsureYouAI.Controllers;

public class CommentController : Controller
{
    private readonly InsureContext _context;
    private readonly IMapper _mapper;

    public CommentController(InsureContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IActionResult CommentList()
    {
        var comments = _context.Comments.ProjectTo<ResultCommentDto>(_mapper.ConfigurationProvider).ToList();

        var result = _mapper.Map<List<ResultCommentDto>>(comments);

        return View(result);
    }
}
