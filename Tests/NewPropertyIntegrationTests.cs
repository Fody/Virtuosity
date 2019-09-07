using System;
using System.Linq;
using System.Reflection;
using Fody;
using Xunit;
using Xunit.Abstractions;

public class NewPropertyIntegrationTests :
    XunitApprovalBase
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
        var weavingTask = new ModuleWeaver();
        assembly = weavingTask.ExecuteTestRun("AssemblyToProcess.dll",
            assemblyName: nameof(NewPropertyIntegrationTests)).Assembly;
    }

    public NewPropertyIntegrationTests(ITestOutputHelper output) :
        base(output)
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

    [Fact]
    public void Get_OnBaseClass_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(baseProperty.GetMethod);
    }

    [Fact]
    public void Set_OnBaseClass_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(baseProperty.SetMethod);
    }

    [Fact]
    public void Get_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedSameBackingProperty.GetMethod);
    }

    [Fact]
    public void Set_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedSameBackingProperty.SetMethod);
    }

    [Fact]
    public void Get_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedNewProperty.GetMethod);
    }

    [Fact]
    public void Set_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(derivedNewProperty.SetMethod);
    }

    [Fact]
    public void Get_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedSameBackingProperty.GetMethod);
    }

    [Fact]
    public void Set_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedSameBackingProperty.SetMethod);
    }

    [Fact]
    public void Get_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedNewProperty.GetMethod);
    }

    [Fact]
    public void Set_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustBeNewVirtualMethod()
    {
        AssertIsNewVirtualMethod(genericDerivedNewProperty.SetMethod);
    }

    [Fact]
    public void Get_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = derivedSameBackingProperty.GetValue(instance);

        Assert.Equal(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Set_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.Equal(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Get_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = derivedNewProperty.GetValue(instance);

        Assert.NotEqual(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Set_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(derivedType);

        derivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.Null(actualValue);
    }

    [Fact]
    public void Get_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = genericDerivedSameBackingProperty.GetValue(instance);

        Assert.Equal(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Set_Generic_WhenPropertyUsesSameBackingFieldAsBase_MustSetValueToBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedSameBackingProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.Equal(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Get_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotGetValueFromBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        baseNewProperty.SetValue(instance, expectedDateTimeOffset);

        var actualValue = genericDerivedNewProperty.GetValue(instance);

        Assert.NotEqual(expectedDateTimeOffset, actualValue);
    }

    [Fact]
    public void Set_Generic_WhenPropertyUsesOtherBackingFieldAsBase_MustNotSetValueOnBase()
    {
        var expectedDateTimeOffset = DateTimeOffset.UtcNow;

        dynamic instance = Activator.CreateInstance(genericDerivedType);

        genericDerivedNewProperty.SetValue(instance, expectedDateTimeOffset);
        var actualValue = baseProperty.GetValue(instance);

        Assert.Null(actualValue);
    }

    static PropertyInfo GetPropertyInfoFromSpecificType(Type type, string name)
    {
        return type
            .GetProperties(PropertyBindingFlags)
            .Where(x => x.DeclaringType == type)
            .SingleOrDefault(x => string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
    }

    static void AssertIsNewVirtualMethod(MethodInfo method)
    {
        Assert.False(method.IsAbstract, $"{method.Name} IsAbstract");
        Assert.True(method.IsHideBySig, $"{method.Name} IsHideBySig");
        Assert.True(method.IsSpecialName, $"{method.Name} IsSpecialName");
        Assert.True(method.IsVirtual, $"{method.Name} IsVirtual");
        Assert.False(method.IsStatic, $"{method.Name} IsStatic");
        Assert.False(method.IsFinal, $"{method.Name} IsFinal");
        Assert.True(method.Attributes.HasFlag(MethodAttributes.NewSlot), $"{method.Name} HasFlag(NewSlot)");
    }
}