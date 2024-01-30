using System;
using System.Reflection;
using Fody;
using Xunit;

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

    [Fact]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        assembly.EnsureMembersAreVirtual("MethodsAndPropertiesAreMarkedAsVirtualClass", "Method1", "Property1");
    }

    [Fact]
    public void NonAbstractMethodsAndPropertiesOnAbstractClassAreMarkedAsVirtual()
    {
        assembly.EnsureMembersAreVirtual("AbstractClass", "NonAbstractMethod", "NonAbstractProperty");
    }

    [Fact]
    public void InterfaceSealedClass()
    {
        assembly.EnsureMembersAreSealed("InterfaceSealedClass", "Property");
        assembly.EnsureMembersAreVirtual("InterfaceSealedClass", "Property");
    }

    [Fact]
    public void EnsureNested()
    {
        assembly.EnsureMembersAreVirtual("EnsureNested.Outer+Inner", "Property");
    }

    [Fact]
    public void EnsureNewToOverrideWithInterface()
    {
        var child = assembly.GetType("EnsureNewToOverrideWithInterface.ChildImplementation");
        var baseProperty = assembly.GetType("EnsureNewToOverrideWithInterface.BaseImplementation").GetProperty("Property", BindingFlags.Public | BindingFlags.Instance);
        var propValue = baseProperty.GetValue(Activator.CreateInstance(child), null);
        Assert.Equal("Bravo", propValue);
    }

    [Fact]
    public void InterfaceVirtualClass()
    {
        assembly.EnsureMembersAreVirtual("InterfaceVirtualClass", "Property");
        assembly.EnsureMembersAreNotSealed("InterfaceVirtualClass", "Property");
    }

    [Fact]
    public void EnsurePropertyCallIsRedirected()
    {
        var type = assembly.GetType("PropertyRedirectionChildClass", true);
        dynamic instance = Activator.CreateInstance(type);
        Assert.Equal("Child", instance.Property1);
    }

    [Fact]
    public void SealedNotMarkedVirtual()
    {
        assembly.EnsureMembersAreNotVirtual("SealedClass", "Method1", "Property1");
    }
}