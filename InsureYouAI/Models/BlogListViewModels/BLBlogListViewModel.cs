using InsureYouAI.Entities;

namespace InsureYouAI.Models.BlogListViewModels;

public class BLBlogListViewModel
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate{ get; set; }
    public string Content { get; set; }
    public string CoverImageUrl { get; set; }
    public string MainCoverImageUrl { get; set; }
    public string CategoryName { get; set; }
    public string Author { get; set; }
    public int CommentCount { get; set; }
}
