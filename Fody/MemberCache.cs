using System.Collections.Generic;
using Mono.Cecil;

public partial class ModuleWeaver
{

    public void AddMethodToCache(MethodDefinition methodDefinition)
    {
        MethodCache.Add(methodDefinition);
    }

    public List<MethodDefinition> MethodCache = new List<MethodDefinition>();
}