namespace InsureYouAI.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PageInfoAttribute : Attribute
{
    public string ControllerName { get; }
    public string PageName { get; }

    public PageInfoAttribute(string controllerName, string pageName)
    {
        ControllerName = controllerName;
        PageName = pageName;
    }
}
