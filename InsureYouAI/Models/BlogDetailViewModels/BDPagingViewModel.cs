namespace InsureYouAI.Models.BlogDetailViewModels;

public class BDPagingViewModel
{
    public int CurrentId { get; set; }
    public int? PreviousId { get; set; }
    public string PreviousTitle { get; set; }
    public int? NextId { get; set; }
    public string NextTitle { get; set; }
}
