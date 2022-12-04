using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public void ConvertNewToOverrides()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {
            if (type.IsInterface)
            {
                continue;
            }
            if (type.IsEnum)
            {
                continue;
            }
            var baseTypes = GetBaseTypes(type).ToList();
            var baseMethods = MethodCache.Where(x => baseTypes.Contains(x.DeclaringType)).ToList();
            if (baseMethods.Count == 0)
            {
                continue;
            }
            foreach (var methodDefinition in type.Methods)
            {
                Replace(methodDefinition, baseMethods);
            }
            foreach (var propertyDefinition in type.Properties)
            {
                Replace(propertyDefinition.GetMethod, baseMethods);
                Replace(propertyDefinition.SetMethod, baseMethods);
            }
        }
    }

    static IEnumerable<TypeReference> GetBaseTypes(TypeDefinition typeDefinition)
    {
        var typeReferences = new List<TypeReference>();
        do
        {
            if (typeDefinition.BaseType == null)
            {
                break;
            }
            if (typeDefinition.BaseType.FullName == "System.Object")
            {
                break;
            }
            if (typeDefinition.BaseType.Module != typeDefinition.Module)
            {
                break;
            }
            typeDefinition = typeDefinition.BaseType.Resolve();
            typeReferences.Add(typeDefinition);
        } while (true);
        return typeReferences;
    }

    static void Replace(MethodDefinition methodDefinition, List<MethodDefinition> baseMethods)
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
        var definition = baseMethods.FirstOrDefault(baseMethod =>
                                                    HaveSameMethodSignature(methodDefinition, baseMethod));
        if (definition == null)
        {
            return;
        }
        methodDefinition.IsNewSlot = false;
    }

    static bool HaveSameMethodSignature(MethodDefinition md1, MethodDefinition md2)
    {
        if (md1.Name != md2.Name
            || md1.MethodReturnType.ReturnType != md2.MethodReturnType.ReturnType)
        {
            return false;
        }

        if (md1.Parameters.Count != md2.Parameters.Count)
        {
            return false;
        }

        for (var i = 0; i < md1.Parameters.Count; i++)
        {
            if (md1.Parameters[i].ParameterType != md2.Parameters[i].ParameterType)
            {
                return false;
            }
        }

        return true;
    }
}