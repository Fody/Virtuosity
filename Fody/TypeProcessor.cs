using Mono.Cecil;

public class TypeProcessor
{
    ModuleWeaver moduleWeaver;
    MemberCache memberCache;

    public TypeProcessor(ModuleWeaver moduleWeaver, MemberCache memberCache)
    {
        this.moduleWeaver = moduleWeaver;
        this.memberCache = memberCache;
    }

    public void Execute(TypeDefinition typeDefinition)
    {
        moduleWeaver.LogInfo("\t" + typeDefinition.FullName);

        foreach (var method in typeDefinition.Methods)
        {
            if (method.IsConstructor)
            {
                continue;
            }
            ProcessMethod(method);
        }
    }

    void ProcessMethod(MethodDefinition method)
    {
        if (method == null)
        {
            return;
        }

        if (method.IsFinal && method.IsVirtual)
        {
            method.IsFinal = false;
            return;
        }
        if (method.IsFinal)
        {
            return;
        }
        if (method.IsVirtual)
        {
            return;
        }
        if (method.IsStatic)
        {
            return;
        }
        if (method.IsVirtual)
        {
            return;
        }
        if (method.IsPrivate)
        {
            return;
        }
        memberCache.AddMethod(method);
        method.IsVirtual = true;
        method.IsNewSlot = true;
    }
}