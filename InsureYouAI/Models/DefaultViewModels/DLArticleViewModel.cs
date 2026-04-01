using InsureYouAI.Entities;

namespace InsureYouAI.Models.DefaultViewModels;

public class DLArticleViewModel
{
    public string Title { get; set; }
    public DateTime CreatedTime { get; set; }
    public string Content { get; set; }
    public string CoverImageUrl { get; set; }
    public string MainCoverImageUrl { get; set; }
    public string CategoryName { get; set; }
    public List<Comment> Comments { get; set; }
}
