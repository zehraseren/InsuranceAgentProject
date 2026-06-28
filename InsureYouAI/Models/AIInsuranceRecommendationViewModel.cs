using InsureYouAI.Enums;
using System.Text.Json.Serialization;

namespace InsureYouAI.Models;

public class AIInsuranceRecommendationViewModel
{
    // Kullanıcıdan alınacak alanlar
    public int? Age { get; set; }
    public string? Occupation { get; set; }
    public string? City { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public int? ChildrenCount { get; set; }
    public TravelFrequency? TravelFrequency { get; set; }
    public decimal? MonthlyBudget { get; set; }
    public bool HasChronicDisease { get; set; }
    public string? ChronicDiseaseDetails { get; set; }
    public InsuranceInterest? InsuranceInterest { get; set; }

    [JsonPropertyName("onerilenPaket")]
    public string? RecommendedPackage { get; set; }

    [JsonPropertyName("ikinciSecenek")]
    public string? SecondBestPackage { get; set; }

    [JsonPropertyName("neden")]
    public string? AnalysisText { get; set; }
}
