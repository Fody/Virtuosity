using NUnit.Framework;

[TestFixture]
public class AssemblyWithIncludesTest
{
    [Test]
    public void Simple()
    {
        var excludeType = typeof(AssemblyWithIncludes.ExcludeNamespace.ExcludeClass);
        Assert.IsFalse(excludeType.GetMethod("Method").IsVirtual);
        var includeType = typeof(AssemblyWithIncludes.IncludeNamespace.IncludeClass);
        Assert.IsTrue(includeType.GetMethod("Method").IsVirtual);

        var inNamespaceButWithAttributeType = typeof(AssemblyWithIncludes.IncludeNamespace.InNamespaceButWithAttributeClass);
        Assert.IsFalse(inNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
        var notInNamespaceButWithAttributeType = typeof(AssemblyWithIncludes.ExcludeNamespace.NotInNamespaceButWithAttributeClass);
        Assert.IsFalse(notInNamespaceButWithAttributeType.GetMethod("Method").IsVirtual);
    }
}