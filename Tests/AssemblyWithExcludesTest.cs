using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class AssemblyWithExcludesTest
{
    [Test]
    public void Simple()
    {
        var beforeAssemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyWithExcludes.dll");

        var afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath))
        {
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
                ExcludeNamespaces = new List<string> { "ExcludeNamespace"}
            };

            weavingTask.Execute();
            moduleDefinition.Write(afterAssemblyPath);
        }

        var assembly = Assembly.LoadFile(afterAssemblyPath);

        var excludeType = assembly.GetType("ExcludeNamespace.ExcludeClass");
        Assert.IsFalse(excludeType.GetMethod("Method").IsVirtual);
        var includeType = assembly.GetType("IncludeNamespace.IncludeClass");
        Assert.IsTrue(includeType.GetMethod("Method").IsVirtual);

        var inNamespaceButWithAttributeType = assembly.GetType("IncludeNamespace.InNamespaceButWithAttributeClass");
        Assert.IsFalse(inNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
        var notInNamespaceButWithAttributeType = assembly.GetType("ExcludeNamespace.NotInNamespaceButWithAttributeClass");
        Assert.IsFalse(notInNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
#if(DEBUG)
        Verifier.Verify(beforeAssemblyPath, afterAssemblyPath);
#endif
    }
}