using System;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    Assembly assembly;

    public IntegrationTests()
    {
        var projectPath = @"AssemblyToProcess\AssemblyToProcess.csproj";
#if (!DEBUG)

            projectPath = projectPath.Replace("Debug", "Release");
#endif
        var weaverHelper = new WeaverHelper(projectPath);
        assembly = weaverHelper.Assembly;
    }

    [Test]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        VirtualTester.EnsureMembersAreVirtual("MethodsAndPropertiesAreMarkedAsVirtualClass", assembly, "Method1", "Property1");
    }
    [Test]
    public void NonAbstractMethodsAndPropertiesOnAbstractClassAreMarkedAsVirtual()
    {
		VirtualTester.EnsureMembersAreVirtual("AbstractClass", assembly, "NonAbstractMethod", "NonAbstractProperty");
    }

    [Test]
    public void InterfaceSealedClass()
    {
        VirtualTester.EnsureMembersAreSealed("InterfaceSealedClass", assembly, "Property");
        VirtualTester.EnsureMembersAreVirtual("InterfaceSealedClass", assembly, "Property");
    }
    [Test]
    public void EnsureNewToOverrideWithInterface()
    {
        var child = assembly.GetType("EnsureNewToOverrideWithInterface.ChildImplementation");
        var baseProperty = assembly.GetType("EnsureNewToOverrideWithInterface.BaseImplementation").GetProperty("Property", BindingFlags.Public | BindingFlags.Instance);
        var propValue = baseProperty.GetValue(Activator.CreateInstance(child), null);
        Assert.AreEqual("Bravo", propValue);
    }

    [Test]
    public void InterfaceVirtualClass()
    {
        VirtualTester.EnsureMembersAreVirtual("InterfaceVirtualClass", assembly, "Property");
        VirtualTester.EnsureMembersAreNotSealed("InterfaceVirtualClass", assembly, "Property");
    }
    [Test]
    public void EnsurePropertyCallIsRedirected()
    {
        var type = assembly.GetType("PropertyRedirectionChildClass", true);
        dynamic instance = Activator.CreateInstance(type);
        Assert.AreEqual("Child", instance.Property1);
    }

    [Test]
    public void SealedNotMarkedVirtual()
    {
        VirtualTester.EnsureMembersAreNotVirtual("SealedClass", assembly, "Method1", "Property1");
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assembly.CodeBase.Remove(0, 8));
    }
#endif

}