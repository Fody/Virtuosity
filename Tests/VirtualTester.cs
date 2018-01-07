using System.Linq;
using System.Reflection;
using Xunit;

public static class VirtualTester
{
    public static void EnsureMembersAreVirtual(string className, Assembly assembly, params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var type = assembly.GetType(className, true);

            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = member as MethodInfo;
                Assert.True(methodInfo.IsVirtual, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                Assert.True(propertyInfo.GetSetMethod().IsVirtual, propertyInfo.Name);
                Assert.True(propertyInfo.GetGetMethod().IsVirtual, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreNotVirtual(string className, Assembly assembly, params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var type = assembly.GetType(className, true);

            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = member as MethodInfo;
                Assert.False(methodInfo.IsVirtual, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.False(setMethod.IsVirtual, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.False(getMethod.IsVirtual, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreSealed(string className, Assembly assembly, params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var type = assembly.GetType(className, true);

            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = member as MethodInfo;
                Assert.True(methodInfo.IsFinal, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.True(setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.True(getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreNotSealed(string className, Assembly assembly, params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var type = assembly.GetType(className, true);

            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = member as MethodInfo;
                Assert.False(methodInfo.IsFinal, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.False(setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.False(getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }
}