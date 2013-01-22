using EnsureNewToOverrideWithInterface;
using NUnit.Framework;

[TestFixture]
public class EnsureNewToOverrideWithInterfaceTests
{

    [Test]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        BaseImplementation instance = new ChildImplementation();
        Assert.AreEqual("Bravo",instance.Property);
    }
}