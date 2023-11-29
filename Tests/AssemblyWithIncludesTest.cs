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
            IncludeNamespaces = ["IncludeNamespace"]
        };
        assembly = weavingTask.ExecuteTestRun("AssemblyWithIncludes.dll").Assembly;
    }

    [Fact]
    public void Simple()
    {
        assembly.EnsureMembersAreNotVirtual("ExcludeNamespace.ExcludeClass", "Method");
        assembly.EnsureMembersAreVirtual("IncludeNamespace.IncludeClass", "Method");
        assembly.EnsureMembersAreNotVirtual("IncludeNamespace.InNamespaceButWithAttribute", "Method");
        assembly.EnsureMembersAreNotVirtual("ExcludeNamespace.NotInNamespaceButWithAttribute", "Method");
    }

    [Fact]
    public void EnsureNested()
    {
        assembly.EnsureMembersAreVirtual("IncludeNamespace.Outer+Inner", "Property");
        assembly.EnsureMembersAreNotVirtual("ExcludeNamespace.Outer+Inner", "Property");
        assembly.EnsureMembersAreNotVirtual("IncludeNamespace.Outer+InnerWithAttribute", "Method");
        assembly.EnsureMembersAreNotVirtual("ExcludeNamespace.Outer+InnerWithAttribute", "Method");
    }
}