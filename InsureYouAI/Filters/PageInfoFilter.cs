using InsureYouAI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InsureYouAI.Filters;

public class PageInfoFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var attribute = context.ActionDescriptor.EndpointMetadata
             .OfType<PageInfoAttribute>()
             .FirstOrDefault();

        if (attribute != null && context.Controller is Controller controller)
        {
            controller.ViewData["ControllerName"] = attribute.ControllerName;
            controller.ViewData["PageName"] = attribute.PageName;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
