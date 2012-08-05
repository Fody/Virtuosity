using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public class CallToCallVirtConverter
{
    MemberCache memberCache;
    List<TypeDefinition> allTypes;

    public CallToCallVirtConverter(MemberCache memberCache, List<TypeDefinition> allTypes)
    {
        this.memberCache = memberCache;
        this.allTypes = allTypes;
    }

    public void Execute()
    {
        foreach (var type in allTypes)
        {
            if (type.IsInterface)
            {
                continue;
            }
            if (type.IsEnum)
            {
                continue;
            }
            foreach (var methodDefinition in type.Methods)
            {
                Replace(methodDefinition);
            }
        }
    }

    void Replace(MethodDefinition methodDefinition)
    {
        if (methodDefinition == null)
        {
            return;
        }
        if (methodDefinition.IsAbstract)
        {
            return;
        }
        //for delegates
        if (methodDefinition.Body == null)
        {
            return;
        }
        foreach (var instruction in methodDefinition.Body.Instructions)
        {
            if (instruction.OpCode != OpCodes.Call)
            {
                continue;
            }

            foreach (var method in memberCache.Methods)
            {
                if (instruction.Operand == method)
                {
                    instruction.OpCode = OpCodes.Callvirt;
                }
            }
        }
    }
}