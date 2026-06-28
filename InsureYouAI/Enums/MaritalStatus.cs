using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Enums;

public enum MaritalStatus
{
    [Display(Name = "Bekar")]
    Single,

    [Display(Name = "Evli")]
    Married
}
