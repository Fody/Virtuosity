using System.Linq;
using System.Reflection;
using Xunit;

public static class VirtualTester
{
    public static void EnsureMembersAreVirtual(string className, Assembly assembly, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Assert.True(methodInfo.IsVirtual, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                Assert.True(setMethod != null && setMethod.IsVirtual, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.True(getMethod != null && getMethod.IsVirtual, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreNotVirtual(string className, Assembly assembly, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Assert.False(methodInfo.IsVirtual, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                Assert.False(setMethod != null && setMethod.IsVirtual, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.False(getMethod != null && getMethod.IsVirtual, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreSealed(string className, Assembly assembly, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Assert.True(methodInfo.IsFinal, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                Assert.True(setMethod != null && setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.True(getMethod != null && getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreNotSealed(string className, Assembly assembly, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Assert.False(methodInfo.IsFinal, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                Assert.False(setMethod != null && setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.False(getMethod != null && getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }
}