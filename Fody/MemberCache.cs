using System.Collections.Generic;
using Mono.Cecil;

public class MemberCache
{
    public MemberCache()
    {
        Methods = new List<MethodDefinition>();
    }

    public void AddMethod(MethodDefinition methodDefinition)
    {
        Methods.Add(methodDefinition);
    }

    public List<MethodDefinition> Methods;
}