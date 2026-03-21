namespace InsureYouAI.Dtos.ArticleDtos;

public class UpdateArticleDto
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedTime { get; set; }
    public string Content { get; set; }
    public string CoverImageUrl { get; set; }
    public string MainCoverImageUrl { get; set; }
    public int CategoryId { get; set; }
}
