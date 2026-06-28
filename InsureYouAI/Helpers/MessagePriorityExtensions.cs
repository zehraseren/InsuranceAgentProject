using InsureYouAI.Enums;
using Microsoft.AspNetCore.Html;

namespace InsureYouAI.Helpers;

public static class MessagePriorityExtensions
{
    public static IHtmlContent GetBadge(this MessagePriority priority)
    {
        var css = priority switch
        {
            MessagePriority.Low => "bg-success",
            MessagePriority.Medium => "bg-warning",
            MessagePriority.High => "bg-danger",
            _ => "bg-secondary"
        };

        return new HtmlString($"<span class='badge {css}'>{priority}</span>");
    }
}
