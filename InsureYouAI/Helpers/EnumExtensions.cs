using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Helpers;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum? value)
    {
        if (value == null) return "Belirtilmedi";

        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();

        return member?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName()
            ?? value.ToString();
    }
}
