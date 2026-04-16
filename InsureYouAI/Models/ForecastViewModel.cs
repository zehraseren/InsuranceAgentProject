namespace InsureYouAI.Models;

public class ForecastViewModel
{
    public List<ForecastItemViewModel> Items { get; set; } = new();
}

public class ForecastItemViewModel
{
    public string Month { get; set; }
    public float Forecasted { get; set; }
    public float LowerBound { get; set; }
    public float UpperBound { get; set; }
}
