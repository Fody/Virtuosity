using System;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class SerializationCallbacksIntegrationTests : IntegrationTestsBase
{
    readonly Type type;

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
    public void MustBeAbleToInstanciateType()
    {
        Activator.CreateInstance(type);
    }

    void AssertUnmodifiedMethod(string methodName)
    {
        var method = type.GetMethod(methodName);

        Assert.IsFalse(method.IsAbstract, $"{method.Name} IsAbstract");
        Assert.IsFalse(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Assert.IsFalse(method.IsVirtual, $"{method.Name} IsVirtual");
        Assert.IsFalse(method.IsStatic, $"{method.Name} IsStatic");
        Assert.IsFalse(method.IsFinal, $"{method.Name} IsFinal");
        Assert.IsFalse(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
        Assert.IsTrue(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Assert.IsTrue(method.IsPublic, $"{method.Name} IsPublic");
    }
}
