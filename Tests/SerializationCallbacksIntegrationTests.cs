using System;
using System.Reflection;
using Fody;
using Xunit;
using Xunit.Abstractions;

public class SerializationCallbacksIntegrationTests :
    XunitLoggingBase
{
    Type type;
    static Assembly assembly;

    static SerializationCallbacksIntegrationTests()
    {
        var weavingTask = new ModuleWeaver();
        assembly = weavingTask.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(SerializationCallbacksIntegrationTests)).Assembly;
    }

    public SerializationCallbacksIntegrationTests(ITestOutputHelper output) :
        base(output)
    {
        // when this fails with a TypeLoadException with message Type 'SerializationCallbackMethods' in assembly '(...)'
        // has method '(...)' which is either static, virtual, abstract or generic, but is marked as being a serialization callback
        // then that's because one of the callbacks has been virtualized
        type = assembly.GetType("SerializationCallbackMethods", true);
    }

    [Fact]
    public void Method_MarkedByOnSerializingAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Serializing");
    }

    [Fact]
    public void Method_MarkedByOnSerializedAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Serialized");
    }

    [Fact]
    public void Method_MarkedByOnDeserializingAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Deserializing");
    }

    [Fact]
    public void Method_MarkedByOnDeserializedAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Deserialized");
    }

    [Fact]
    public void MustBeAbleToInstantiateType()
    {
        Activator.CreateInstance(type);
    }

    void AssertUnmodifiedMethod(string methodName)
    {
        var method = type.GetMethod(methodName);

        Assert.False(method.IsAbstract, $"{method.Name} IsAbstract");
        Assert.False(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Assert.False(method.IsVirtual, $"{method.Name} IsVirtual");
        Assert.False(method.IsStatic, $"{method.Name} IsStatic");
        Assert.False(method.IsFinal, $"{method.Name} IsFinal");
        Assert.False(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
        Assert.True(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Assert.True(method.IsPublic, $"{method.Name} IsPublic");
    }
}
