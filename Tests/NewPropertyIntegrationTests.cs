using System;
using System.Linq;
using System.Reflection;
using Fody;

public class NewPropertyIntegrationTests
{
    const string NewProperty_SameBackingField_PropertyName = "Value";
    const string NewProperty_DifferentBackingField_PropertyName = "ReplacedValue";

    const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    Type derivedType;
    Type genericDerivedType;

    PropertyInfo baseProperty;
    PropertyInfo baseNewProperty;

    PropertyInfo derivedSameBackingProperty;
    PropertyInfo derivedNewProperty;

    PropertyInfo genericDerivedSameBackingProperty;
    PropertyInfo genericDerivedNewProperty;

    static Assembly assembly;

    static NewPropertyIntegrationTests()
    {
        var weaver = new ModuleWeaver();
        assembly = weaver.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(NewPropertyIntegrationTests)).Assembly;
    }

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
    public async Task Get_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        object actualValue = (object)derivedSameBackingProperty.GetValue(instance);

        await Assert.That(actualValue).IsEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Set_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        object actualValue = (object)baseProperty.GetValue(instance);

        await Assert.That(actualValue).IsEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Get_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        object actualValue = (object)derivedNewProperty.GetValue(instance);

        await Assert.That(actualValue).IsNotEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Set_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        object actualValue = (object)baseProperty.GetValue(instance);

        await Assert.That(actualValue).IsNull();
    }

    [Test]
    public async Task Get_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        object actualValue = (object)genericDerivedSameBackingProperty.GetValue(instance);

        await Assert.That(actualValue).IsEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Set_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        object actualValue = (object)baseProperty.GetValue(instance);

        await Assert.That(actualValue).IsEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Get_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        object actualValue = (object)genericDerivedNewProperty.GetValue(instance);

        await Assert.That(actualValue).IsNotEqualTo(expectedDateTimeOffset);
    }

    [Test]
    public async Task Set_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        object actualValue = (object)baseProperty.GetValue(instance);

        await Assert.That(actualValue).IsNull();
    }

    static PropertyInfo GetPropertyInfoFromSpecificType(Type type, string name)
    {
        return type
            .GetProperties(PropertyBindingFlags)
            .Where(_ => _.DeclaringType == type)
            .SingleOrDefault(x => string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    static void AssertIsNewVirtualMethod(MethodInfo method)
    {
        Check.False(method.IsAbstract, $"{method.Name} IsAbstract");
        Check.True(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Check.True(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Check.True(method.IsVirtual, $"{method.Name} IsVirtual");
        Check.False(method.IsStatic, $"{method.Name} IsStatic");
        Check.False(method.IsFinal, $"{method.Name} IsFinal");
        Check.True(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
    }
}