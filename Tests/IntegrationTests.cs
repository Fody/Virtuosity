using NUnit.Framework;


[TestFixture]
public class IntegrationTests
{

    [Test]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        VirtualTester.EnsureMembersAreVirtual<MethodsAndPropertiesAreMarkedAsVirtualClass>("Method1", "Property1");
    }

    [Test]
    public void NonAbstractMethodsAndPropertiesOnAbstractClassAreMarkedAsVirtual()
    {
        VirtualTester.EnsureMembersAreVirtual<AbstractClass>("NonAbstractMethod", "NonAbstractProperty");
    }

    [Test]
    public void InterfaceSealedClass()
    {
        VirtualTester.EnsureMembersAreSealed<InterfaceSealedClass>("Property");
        VirtualTester.EnsureMembersAreVirtual<InterfaceSealedClass>("Property");
    }

    [Test]
    public void InterfaceVirtualClass()
    {
        VirtualTester.EnsureMembersAreVirtual<InterfaceVirtualClass>("Property");
        VirtualTester.EnsureMembersAreNotSealed<InterfaceVirtualClass>("Property");
    }

    [Test]
    public void EnsurePropertyCallIsRedirected()
    {
        Assert.AreEqual("Child", new PropertyRedirectionChildClass().Property1);
    }

    [Test]
    public void SealedNotMarkedVirtual()
    {
        VirtualTester.EnsureMembersAreNotVirtual<SealedClass>("Method1", "Property1");
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        //Verifier.Verify( assembly.CodeBase.Remove(0, 8));
    }
#endif

}