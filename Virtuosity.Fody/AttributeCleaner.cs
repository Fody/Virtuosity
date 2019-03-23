using System.Linq;

public partial class ModuleWeaver
{
    public void CleanAttributes()
    {
        foreach (var type in ModuleDefinition.GetAllClasses())
        {
            var attributes = type.CustomAttributes;
            var attribute = attributes.SingleOrDefault(x => x.AttributeType.FullName == "Virtuosity.DoNotVirtualizeAttribute");
            if (attribute != null)
            {
                attributes.Remove(attribute);
            }
        }
    }
}