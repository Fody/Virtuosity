using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public static class CecilExtensions
{
    public static bool ContainsAttribute(this IEnumerable<CustomAttribute> attributes, string attributeName) =>
        attributes.Any(attribute => attribute.Constructor.DeclaringType.Name == attributeName);

    public static string GetNamespace(this TypeDefinition type)
    {
        if (type.IsNested)
        {
            return type.DeclaringType.Namespace;
        }

        return type.Namespace;
    }

    public static List<TypeDefinition> GetAllClasses(this ModuleDefinition moduleDefinition)
    {
        var definitions = new List<TypeDefinition>();
        //First is always module so we will skip that;
        GetTypes(moduleDefinition.Types.Skip(1), definitions);
        return definitions;
    }

    static void GetTypes(IEnumerable<TypeDefinition> typeDefinitions, List<TypeDefinition> definitions)
    {
        foreach (var typeDefinition in typeDefinitions)
        {
            GetTypes(typeDefinition.NestedTypes, definitions);

            if (typeDefinition.IsInterface)
            {
                continue;
            }
            if (typeDefinition.IsEnum)
            {
                continue;
            }
            definitions.Add(typeDefinition);
        }
    }
}