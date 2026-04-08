namespace InsureYouAI.Models.BlogDetailViewModels;

public class BDArticleContentViewModel
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Content { get; set; }
    public string CategoryName { get; set; }
    public string Author { get; set; }
    public int CommentCount { get; set; }
}
