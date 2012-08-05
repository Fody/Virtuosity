using System.Collections.Generic;
using Mono.Cecil;

public class AssemblyProcessor
{
    TypeProcessor typeProcessor;
    InclusionChecker inclusionChecker;
    List<TypeDefinition> allTypes;

    public AssemblyProcessor(TypeProcessor typeProcessor, InclusionChecker inclusionChecker, List<TypeDefinition> allTypes)
    {
        this.typeProcessor = typeProcessor;
        this.inclusionChecker = inclusionChecker;
        this.allTypes = allTypes;
    }

    public void Execute()
    {
        foreach (var type in allTypes)
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