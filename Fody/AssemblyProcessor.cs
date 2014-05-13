using Mono.Cecil;

public partial class ModuleWeaver
{

    public void ProcessAssembly()
    {
        foreach (var type in ModuleDefinition.GetAllClasses())
        {
            if (!ShouldInclude(type))
            {
                continue;
            }
            if (ShouldIncludeType(type))
            {
                ProcessType(type);
            }
        }
    }

    bool ShouldInclude(TypeDefinition type)
    {
        if (type.IsSealed)
        {
            return false;
        }
        return true;
    }
}