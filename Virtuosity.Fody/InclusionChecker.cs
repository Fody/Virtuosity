using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public Func<TypeDefinition, bool> ShouldIncludeType;
    List<LineMatcher> matchers;

    public void ProcessIncludesExcludes()
    {
        if (ExcludeNamespaces.Any())
        {
            matchers = GetLines(ExcludeNamespaces).ToList();
            ShouldIncludeType = type =>
            {
                type = CheckNested(type);
                return matchers.All(matcher => !matcher.Match(type.Namespace)) &&
                       !ContainsIgnoreAttribute(type);
            };
            return;
        }

        if (IncludeNamespaces.Any())
        {
            matchers = GetLines(IncludeNamespaces).ToList();
            ShouldIncludeType = type =>
            {
                type = CheckNested(type);
                return matchers.Any(lineMatcher => lineMatcher.Match(type.Namespace)) &&
                       !ContainsIgnoreAttribute(type);
            };
            return;
        }

        ShouldIncludeType = type => !ContainsIgnoreAttribute(type);
    }

    TypeDefinition CheckNested(TypeDefinition type)
    {
        if (type.IsNested)
        {
            return type.DeclaringType;
        }

        return type;
    }

    bool ContainsIgnoreAttribute(TypeDefinition type)
    {
        if (type.CustomAttributes.ContainsAttribute("DoNotVirtualizeAttribute"))
        {
            return true;
        }

        if (type.IsNested)
        {
            return type.DeclaringType.CustomAttributes.ContainsAttribute("DoNotVirtualizeAttribute");
        }

        return false;
    }

    public static IEnumerable<LineMatcher> GetLines(List<string> namespaces)
    {
        return namespaces.Select(BuildLineMatcher);
    }

    public static LineMatcher BuildLineMatcher(string line)
    {
        var starStart = false;
        if (line.StartsWith("*"))
        {
            starStart = true;
            line = line.Substring(1);
        }

        var starEnd = false;
        if (line.EndsWith("*"))
        {
            starEnd = true;
            line = line.Substring(0, line.Length - 1);
        }

        ValidateLine(line);
        return new LineMatcher
        {
            Line = line,
            StarStart = starStart,
            StarEnd = starEnd,
        };
    }

    // ReSharper disable once UnusedParameter.Local
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