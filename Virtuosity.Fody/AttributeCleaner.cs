using System.Linq;

public partial class ModuleWeaver
{
    public void CleanAttributes()
    {
        foreach (var type in allClasses)
        {
            var attributes = type.CustomAttributes;
            var attribute = attributes.SingleOrDefault(_ => _.AttributeType.FullName == "Virtuosity.DoNotVirtualizeAttribute");
            if (attribute != null)
            {
                attributes.Remove(attribute);
            }
        }
    }
}