using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class NewPropertyIntegrationTests : IntegrationTestsBase
{
    const string NewProperty_SameBackingField_PropertyName = "Value";
    const string NewProperty_DifferentBackingField_PropertyName = "ReplacedValue";

    const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    readonly Type derivedType;
    readonly Type genericDerivedType;

    readonly PropertyInfo baseProperty;
    readonly PropertyInfo baseNewProperty;

    readonly PropertyInfo derivedSameBackingProperty;
    readonly PropertyInfo derivedNewProperty;

    readonly PropertyInfo genericDerivedSameBackingProperty;
    readonly PropertyInfo genericDerivedNewProperty;
    
    public NewPropertyIntegrationTests()
    {
        var baseType = assembly.GetType("NewProperty.BaseProperty", true);
        derivedType = assembly.GetType("NewProperty.DateTimeOffsetProperty", true);
        genericDerivedType = assembly.GetType("NewProperty.GenericProperty`1", true).MakeGenericType(typeof(DateTimeOffset));
        
        baseProperty = GetPropertyInfoFromSpecificType(baseType, NewProperty_SameBackingField_PropertyName);
        baseNewProperty = GetPropertyInfoFromSpecificType(baseType, NewProperty_DifferentBackingField_PropertyName);

        derivedSameBackingProperty = GetPropertyInfoFromSpecificType(derivedType, NewProperty_SameBackingField_PropertyName);
        derivedNewProperty = GetPropertyInfoFromSpecificType(derivedType, NewProperty_DifferentBackingField_PropertyName);

        genericDerivedSameBackingProperty = GetPropertyInfoFromSpecificType(genericDerivedType, NewProperty_SameBackingField_PropertyName);
        genericDerivedNewProperty = GetPropertyInfoFromSpecificType(genericDerivedType, NewProperty_DifferentBackingField_PropertyName);
    }

    [Test]
    public void Get_OnBaseClass_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(baseProperty.GetMethod);
    }

    [Test]
    public void Set_OnBaseClass_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(baseProperty.SetMethod);
    }

    [Test]
    public void Get_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedSameBackingProperty.GetMethod);
    }

    [Test]
    public void Set_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedSameBackingProperty.SetMethod);
    }


    [Test]
    public void Get_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedNewProperty.GetMethod);
    }

    [Test]
    public void Set_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedNewProperty.SetMethod);
    }

    [Test]
    public void Get_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedSameBackingProperty.GetMethod);
    }

    [Test]
    public void Set_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedSameBackingProperty.SetMethod);
    }


    [Test]
    public void Get_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedNewProperty.GetMethod);
    }

    [Test]
    public void Set_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedNewProperty.SetMethod);
    }

    [Test]
    public void Get_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;
        
        dynamic instance = Activator.CreateInstance(derivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = derivedSameBackingProperty.GetValue(instance);

        Assert.AreEqual(expectedDateTimeOffset, actualValue);
    }

    [Test]
    public void Set_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.AreEqual(expectedDateTimeOffset, actualValue);
    }


    [Test]
    public void Get_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = derivedNewProperty.GetValue(instance);

        Assert.AreNotEqual(expectedDateTimeOffset, actualValue);
    }

    [Test]
    public void Set_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.AreNotEqual(expectedDateTimeOffset, actualValue);
    }

    [Test]
    public void Get_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = genericDerivedSameBackingProperty.GetValue(instance);

        Assert.AreEqual(expectedDateTimeOffset, actualValue);
    }

    [Test]
    public void Set_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.AreEqual(expectedDateTimeOffset, actualValue);
    }


    [Test]
    public void Get_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = genericDerivedNewProperty.GetValue(instance);

        Assert.AreNotEqual(expectedDateTimeOffset, actualValue);
    }

    [Test]
    public void Set_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.AreNotEqual(expectedDateTimeOffset, actualValue);
    }


    static PropertyInfo GetPropertyInfoFromSpecificType(Type type, string name)
    {
        return type
            .GetProperties(PropertyBindingFlags)
            .Where(x => x.DeclaringType == type)
            .SingleOrDefault(x => string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    private static void AssertIsNewVirtualMethod(MethodInfo method)
    {
        Assert.IsFalse(method.IsAbstract, $"{method.Name} IsAbstract");
        Assert.IsTrue(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Assert.IsTrue(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Assert.IsTrue(method.IsVirtual, $"{method.Name} IsVirtual");
        Assert.IsFalse(method.IsStatic, $"{method.Name} IsStatic");
        Assert.IsFalse(method.IsFinal, $"{method.Name} IsFinal");
        Assert.IsTrue(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
    }
}
