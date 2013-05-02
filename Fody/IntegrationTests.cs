using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    Assembly assembly;
    string beforeAssemblyPath;
    string afterAssemblyPath;

    public IntegrationTests()
    {
        beforeAssemblyPath = Path.GetFullPath(@"..\..\..\AssemblyToProcess\bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)

        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        var moduleDefinition = ModuleDefinition.ReadModule(afterAssemblyPath);
        var weavingTask = new ModuleWeaver
        {
            ModuleDefinition = moduleDefinition,
        };

        weavingTask.Execute();
        moduleDefinition.Write(afterAssemblyPath);

        assembly = Assembly.LoadFile(afterAssemblyPath);
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
        Verifier.Verify(beforeAssemblyPath,afterAssemblyPath);
    }
#endif

}

