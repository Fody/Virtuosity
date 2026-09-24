using System;
using System.Reflection;
using Fody;

public class SerializationCallbacksIntegrationTests
{
    Type type;
    static Assembly assembly;

    static SerializationCallbacksIntegrationTests()
    {
        var weaver = new ModuleWeaver();
        assembly = weaver.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(SerializationCallbacksIntegrationTests)).Assembly;
    }

    public SerializationCallbacksIntegrationTests()
    {
        // when this fails with a TypeLoadException with message Type 'SerializationCallbackMethods' in assembly '(...)'
        // has method '(...)' which is either static, virtual, abstract or generic, but is marked as being a serialization callback
        // then that's because one of the callbacks has been virtualized
        type = assembly.GetType("SerializationCallbackMethods", true);
    }

    [Test]
    public void Method_MarkedByOnSerializingAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Serializing");
    }

    [Test]
    public void Method_MarkedByOnSerializedAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Serialized");
    }

    [Test]
    public void Method_MarkedByOnDeserializingAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Deserializing");
    }

    [Test]
    public void Method_MarkedByOnDeserializedAttribute_MustNotMakeVirtual()
    {
        AssertUnmodifiedMethod("Deserialized");
    }

    [Test]
    public void MustBeAbleToInstantiateType()
    {
        Activator.CreateInstance(type);
    }

    void AssertUnmodifiedMethod(string methodName)
    {
        var method = type.GetMethod(methodName);

        Check.False(method.IsAbstract, $"{method.Name} IsAbstract");
        Check.False(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Check.False(method.IsVirtual, $"{method.Name} IsVirtual");
        Check.False(method.IsStatic, $"{method.Name} IsStatic");
        Check.False(method.IsFinal, $"{method.Name} IsFinal");
        Check.False(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
        Check.True(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Check.True(method.IsPublic, $"{method.Name} IsPublic");
    }
}
