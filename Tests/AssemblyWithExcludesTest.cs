using NUnit.Framework;

[TestFixture]
public class AssemblyWithExcludesTest
{
    [Test]
    public void Simple()
    {
        var excludeType = typeof(AssemblyWithExcludes.ExcludeNamespace.ExcludeClass);
        Assert.IsFalse(excludeType.GetMethod("Method").IsVirtual);
        var includeType = typeof(AssemblyWithExcludes.IncludeNamespace.IncludeClass);
        Assert.IsTrue(includeType.GetMethod("Method").IsVirtual);

        var inNamespaceButWithAttributeType = typeof(AssemblyWithExcludes.IncludeNamespace.InNamespaceButWithAttributeClass);
        Assert.IsFalse(inNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
        var notInNamespaceButWithAttributeType = typeof(AssemblyWithExcludes.ExcludeNamespace.NotInNamespaceButWithAttributeClass);
        Assert.IsFalse(notInNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
    }
}