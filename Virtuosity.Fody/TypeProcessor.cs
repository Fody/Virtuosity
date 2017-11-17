using Mono.Cecil;

public partial class ModuleWeaver
{
    public void ProcessType(TypeDefinition typeDefinition)
    {
        LogDebug($"\t{typeDefinition.FullName}");

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
            AddMethodToCache(method);
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
        if (method.IsPrivate)
        {
            return;
        }

        if (MethodIsSerializationCallback(method))
        {
            return;
        }

        AddMethodToCache(method);
        method.IsVirtual = true;
        method.IsNewSlot = true;
    }

    bool MethodIsSerializationCallback(MethodDefinition method)
    {
        return method.CustomAttributes.ContainsAttribute("OnSerializingAttribute")
               || method.CustomAttributes.ContainsAttribute("OnSerializedAttribute")
               || method.CustomAttributes.ContainsAttribute("OnDeserializingAttribute")
               || method.CustomAttributes.ContainsAttribute("OnDeserializedAttribute");
    }
}