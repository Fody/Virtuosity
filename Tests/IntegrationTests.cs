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
        var weavingTask = new ModuleWeaver();
        assembly = weavingTask.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(IntegrationTests)).Assembly;
    }

    [Fact]
    public void MethodsAndPropertiesAreMarkedAsVirtual()
    {
        VirtualTester.EnsureMembersAreVirtual("MethodsAndPropertiesAreMarkedAsVirtualClass", assembly, "Method1", "Property1");
    }

    [Fact]
    public void NonAbstractMethodsAndPropertiesOnAbstractClassAreMarkedAsVirtual()
    {
        VirtualTester.EnsureMembersAreVirtual("AbstractClass", assembly, "NonAbstractMethod", "NonAbstractProperty");
    }

    [Fact]
    public void InterfaceSealedClass()
    {
        VirtualTester.EnsureMembersAreSealed("InterfaceSealedClass", assembly, "Property");
        VirtualTester.EnsureMembersAreVirtual("InterfaceSealedClass", assembly, "Property");
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
        VirtualTester.EnsureMembersAreVirtual("InterfaceVirtualClass", assembly, "Property");
        VirtualTester.EnsureMembersAreNotSealed("InterfaceVirtualClass", assembly, "Property");
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
        VirtualTester.EnsureMembersAreNotVirtual("SealedClass", assembly, "Method1", "Property1");
    }
}