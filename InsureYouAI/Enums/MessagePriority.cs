using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Enums;

public enum MessagePriority
{
    [Display(Name = "Düşük")]
    Low,
    
    [Display(Name = "Orta")]
    Medium,

    [Display(Name = "Yüksek")]
    High
}
