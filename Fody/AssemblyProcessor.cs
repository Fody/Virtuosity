using Mono.Cecil;

public partial class ModuleWeaver
{

    public void ProcessAssembly()
    {
        foreach (var type in ModuleDefinition.GetTypes())
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
        if (!type.IsClass)
        {
            return false;
        }
        if (type.IsSealed)
        {
            return false;
        }
        return true;
    }
}