using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public partial class ModuleWeaver
{

    public void ConvertFunctionPointer()
    {
        foreach (var type in ModuleDefinition.GetAllClasses())
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

            foreach (var method in MethodCache)
            {
                if (instruction.Operand != method)
                {
                    continue;
                }
                if (!foundUsageInMethod)
                {
                    methodDefinition.Body.SimplifyMacros();
                    foundUsageInMethod = true;
                }
                index++;
                instructions.Insert(index, Instruction.Create(OpCodes.Ldvirtftn, method));
                instruction.OpCode = OpCodes.Dup;
                instruction.Operand = null;
                break;
            }
        }

        if (foundUsageInMethod)
        {
            methodDefinition.Body.OptimizeMacros();
        }
    }
}