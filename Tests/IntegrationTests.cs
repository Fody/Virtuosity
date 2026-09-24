using System;
using System.Reflection;
using Fody;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

public class IntegrationTests
{
    static Assembly assembly;

    static IntegrationTests()
    {
        var weaver = new ModuleWeaver();
        assembly = weaver.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(IntegrationTests)).Assembly;
    }

    [Test]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        assembly.EnsureMembersAreVirtual("MethodsAndPropertiesAreMarkedAsVirtualClass", "Method1", "Property1");
    }

    [Test]
    public void NonAbstractMethodsAndPropertiesOnAbstractClassAreMarkedAsVirtual()
    {
        assembly.EnsureMembersAreVirtual("AbstractClass", "NonAbstractMethod", "NonAbstractProperty");
    }

    [Test]
    public void InterfaceSealedClass()
    {
        assembly.EnsureMembersAreSealed("InterfaceSealedClass", "Property");
        assembly.EnsureMembersAreVirtual("InterfaceSealedClass", "Property");
    }

    [Test]
    public void EnsureNested()
    {
        assembly.EnsureMembersAreVirtual("EnsureNested.Outer+Inner", "Property");
    }

    [Test]
    public async Task EnsureNewToOverrideWithInterface()
    {
        var child = assembly.GetType("EnsureNewToOverrideWithInterface.ChildImplementation");
        var baseProperty = assembly.GetType("EnsureNewToOverrideWithInterface.BaseImplementation").GetProperty("Property", BindingFlags.Public | BindingFlags.Instance);
        var propValue = baseProperty.GetValue(Activator.CreateInstance(child), null);
        await Assert.That(propValue).IsEqualTo("Bravo");
    }

    [Test]
    public void InterfaceVirtualClass()
    {
        assembly.EnsureMembersAreVirtual("InterfaceVirtualClass", "Property");
        assembly.EnsureMembersAreNotSealed("InterfaceVirtualClass", "Property");
    }

    [Test]
    public async Task EnsurePropertyCallIsRedirected()
    {
        var type = assembly.GetType("PropertyRedirectionChildClass", true);
        dynamic instance = Activator.CreateInstance(type);
        await Assert.That((string)instance.Property1).IsEqualTo("Child");
    }

    [Test]
    public void SealedNotMarkedVirtual()
    {
        assembly.EnsureMembersAreNotVirtual("SealedClass", "Method1", "Property1");
    }
}