using InsureYouAI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsureYouAI.Models;

public class UpdateArticleViewModel
{
    public UpdateArticleDto Article { get; set; } = new();
    public List<SelectListItem> Categories { get; set; } = new();
    public List<SelectListItem> Authors { get; set; } = new();
}
