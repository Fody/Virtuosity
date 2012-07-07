using Mono.Cecil;

public class AssemblyProcessor
{
    TypeProcessor typeProcessor;
    InclusionChecker inclusionChecker;
    ModuleDefinition moduleDefinition;

    public AssemblyProcessor(TypeProcessor typeProcessor, InclusionChecker inclusionChecker, ModuleDefinition moduleDefinition)
    {
        this.typeProcessor = typeProcessor;
        this.inclusionChecker = inclusionChecker;
        this.moduleDefinition = moduleDefinition;
    }

    public void Execute()
    {
        foreach (var type in moduleDefinition.GetAllTypeDefinitions())
        {
            if (!ShouldInclude(type))
            {
                continue;
            }
            if (inclusionChecker.ShouldIncludeType(type))
            {
                typeProcessor.Execute(type);
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