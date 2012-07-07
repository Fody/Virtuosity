using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public class NewToOverideConverter
{
    MemberCache memberCache;
    ModuleWeaver moduleWeaver;
    ModuleDefinition moduleDefinition;

    public NewToOverideConverter(MemberCache memberCache, ModuleWeaver moduleWeaver, ModuleDefinition moduleDefinition)
    {
        this.memberCache = memberCache;
        this.moduleWeaver = moduleWeaver;
        this.moduleDefinition = moduleDefinition;
    }

    public void Execute()
    {
        
        foreach (var type in moduleDefinition.GetAllTypeDefinitions())
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
            var baseMethods = memberCache.Methods.Where(x => baseTypes.Contains(x.DeclaringType)).ToList();
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

    IEnumerable<TypeReference> GetBaseTypes(TypeDefinition typeDefinition)
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

    void Replace(MethodDefinition methodDefinition, List<MethodDefinition> baseMethods)
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
        var definition = baseMethods.FirstOrDefault(x =>
                                                    x.Name == methodDefinition.Name
                                                    && x.MethodReturnType.ReturnType == methodDefinition.MethodReturnType.ReturnType);
        if (definition == null)
        {
            return;
        }
        methodDefinition.IsNewSlot = false;
    }
}