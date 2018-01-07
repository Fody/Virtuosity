using System.Collections.Generic;
using Fody;
using Xunit;

public class AssemblyWithExcludesTest
{
    [Fact]
    public void Simple()
    {
        var weavingTask = new ModuleWeaver
        {
            ExcludeNamespaces = new List<string> { "ExcludeNamespace" }
        };
        var assembly = weavingTask.ExecuteTestRun("AssemblyWithExcludes.dll").Assembly;

        var excludeType = assembly.GetType("ExcludeNamespace.ExcludeClass");
        Assert.False(excludeType.GetMethod("Method").IsVirtual);
        var includeType = assembly.GetType("IncludeNamespace.IncludeClass");
        Assert.True(includeType.GetMethod("Method").IsVirtual);

        var inNamespaceButWithAttributeType = assembly.GetType("IncludeNamespace.InNamespaceButWithAttributeClass");
        Assert.False(inNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
        var notInNamespaceButWithAttributeType = assembly.GetType("ExcludeNamespace.NotInNamespaceButWithAttributeClass");
        Assert.False(notInNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
    }
}