using System.Collections.Generic;
using System.Reflection;
using Fody;
using Xunit;

public class AssemblyWithIncludesTest
{
    static Assembly assembly;

    static AssemblyWithIncludesTest()
    {
        var weavingTask = new ModuleWeaver
        {
            IncludeNamespaces = new List<string> { "IncludeNamespace" }
        };
        assembly = weavingTask.ExecuteTestRun("AssemblyWithIncludes.dll",
            assemblyName: nameof(IntegrationTests)).Assembly;
    }

    [Fact]
    public void Simple()
    {
        VirtualTester.EnsureMembersAreNotVirtual("ExcludeNamespace.ExcludeClass", assembly, "Method");
        VirtualTester.EnsureMembersAreVirtual("IncludeNamespace.IncludeClass", assembly, "Method");
        VirtualTester.EnsureMembersAreNotVirtual("IncludeNamespace.InNamespaceButWithAttributeClass", assembly, "Method");
        VirtualTester.EnsureMembersAreNotVirtual("ExcludeNamespace.NotInNamespaceButWithAttributeClass", assembly, "Method");
    }
    }
}