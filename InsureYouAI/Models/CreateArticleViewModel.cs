using InsureYouAI.Dtos.ArticleDtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InsureYouAI.Models;

public class CreateArticleViewModel
{
    // Includes DTO properties
    public CreateArticleDto Article { get; set; }

    // Dropdowns for the UI
    [BindNever]
    public List<SelectListItem> Categories { get; set; }
    [BindNever]
    public List<SelectListItem> Authors { get; set; }
}
