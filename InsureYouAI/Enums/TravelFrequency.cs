using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Enums;

public enum TravelFrequency
{
    [Display(Name = "Hiç")]
    Never,

    [Display(Name = "Yılda Birkaç Kez")]
    FewTimesPerYear,

    [Display(Name = "Ayda Bir")]
    Monthly,

    [Display(Name = "Sık Sık")]
    Frequent
}
