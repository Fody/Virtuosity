using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public partial class ModuleWeaver
{
    public void ConvertFunctionPointer()
    {
        foreach (var type in allClasses)
        {
            foreach (var methodDefinition in type.Methods)
            {
                ReplaceUnmanaged(methodDefinition);
            }
        }
    }

    void ReplaceUnmanaged(MethodDefinition methodDefinition)
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
        methodDefinition.Body.SimplifyMacros();
        var instructions = methodDefinition.Body.Instructions;
        var foundUsageInMethod = false;
        for (var index = 0; index < instructions.Count; index++)
        {
            var instruction = instructions[index];
            if (instruction.OpCode != OpCodes.Ldftn)
            {
                continue;
            }

            var methodToLdvirtfn = DetermineMethodTo_Ldvirtftn(instruction.Operand);
            if (methodToLdvirtfn == null)
            {
                continue;
            }

            if (!foundUsageInMethod)
            {
                methodDefinition.Body.SimplifyMacros();
                foundUsageInMethod = true;
            }

            index++;
            instructions.Insert(index, Instruction.Create(OpCodes.Ldvirtftn, methodToLdvirtfn));
            instruction.OpCode = OpCodes.Dup;
            instruction.Operand = null;
        }

        if (foundUsageInMethod)
        {
            methodDefinition.Body.OptimizeMacros();
        }
    }

    MethodReference DetermineMethodTo_Ldvirtftn(object operand)
    {
        foreach (var method in MethodCache)
        {
            if (operand == method)
            {
                return method;
            }

            if (operand is MethodReference operandMethodReference)
            {
                var operandMethodDefinition = operandMethodReference.Resolve();
                if (operandMethodDefinition == method)
                {
                    return operandMethodReference;
                }
            }
        }

        return null;
    }
}