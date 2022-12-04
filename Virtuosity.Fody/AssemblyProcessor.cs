using Mono.Cecil;

public partial class ModuleWeaver
{
    public void ProcessAssembly()
    {
        foreach (var type in allClasses)
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

    static bool ShouldInclude(TypeDefinition type)
    {
        if (type.IsSealed)
        {
            return false;
        }
        return true;
    }
}