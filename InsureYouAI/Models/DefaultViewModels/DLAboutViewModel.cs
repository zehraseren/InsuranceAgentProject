using InsureYouAI.Entities;

namespace InsureYouAI.Models.DefaultViewModels;

public class DLAboutViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public List<string> Details { get; set; }
}
