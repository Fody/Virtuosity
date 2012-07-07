using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public class InclusionChecker
{
    ModuleWeaver moduleWeaver;
    public Func<TypeDefinition, bool> ShouldIncludeType;
    List<LineMatcher> lineMatchers;

    public InclusionChecker(ModuleWeaver moduleWeaver)
    {
        this.moduleWeaver = moduleWeaver;
    }

    public void Execute()
    {
        if (moduleWeaver.ExcludeNamespaces != null && moduleWeaver.IncludeNamespaces != null)
        {
            throw new Exception("Only ExcludeNamespaces OR IncludeNamespaces allowed.");
        }
        if (moduleWeaver.ExcludeNamespaces != null)
        {
            lineMatchers = GetLines(moduleWeaver.ExcludeNamespaces).ToList();
            ShouldIncludeType = definition => lineMatchers.All(lineMatcher => !lineMatcher.Match(definition.Namespace)) && !ContainsIgnoreAttribute(definition);
            return;
        }
        if (moduleWeaver.IncludeNamespaces != null)
        {
            lineMatchers = GetLines(moduleWeaver.IncludeNamespaces).ToList();
            ShouldIncludeType = definition => lineMatchers.Any(lineMatcher => lineMatcher.Match(definition.Namespace)) && !ContainsIgnoreAttribute(definition);
            return;
        }
        ShouldIncludeType = definition => !ContainsIgnoreAttribute(definition);
    }

    bool ContainsIgnoreAttribute(TypeDefinition typeDefinition)
    {
        return typeDefinition.CustomAttributes.ContainsAttribute("DoNotVirtualizeAttribute");
    }

    public static IEnumerable<LineMatcher> GetLines(string namespaces)
    {
        return namespaces.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).Select(BuildLineMatcher);
    }

    public static LineMatcher BuildLineMatcher(string line)
    {
        var tempLine = line.Trim();

        var starStart = false;
        if (tempLine.StartsWith("*"))
        {
            starStart = true;
            tempLine = tempLine.Substring(1);
        }

        var starEnd = false;
        if (tempLine.EndsWith("*"))
        {
            starEnd = true;
            tempLine = tempLine.Substring(0, tempLine.Length - 1);
        }

        ValidateLine(tempLine);
        return new LineMatcher
                   {
                       Line = tempLine,
                       StarStart = starStart,
                       StarEnd = starEnd,
                   };
    }

    static void ValidateLine(string line)
    {
        if (line.Contains("*"))
        {
            throw new Exception("Namespaces can't only start or end with '*'.");
        }
        if (line.Contains(" "))
        {
            throw new Exception("Namespaces cant contain spaces.");
        }
    }
}