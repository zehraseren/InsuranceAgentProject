using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Enums;

public enum InsuranceInterest
{
    [Display(Name = "Sağlık")]
    Health,

    [Display(Name = "Araç")]
    Vehicle,

    [Display(Name = "Seyahat")]
    Travel,

    [Display(Name = "Genel Koruma")]
    GeneralProtection
}
