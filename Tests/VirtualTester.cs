using System.Linq;
using System.Reflection;
using NUnit.Framework;

public static class VirtualTester
{
    public static void EnsureMembersAreVirtual<T>(params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var member = typeof(T).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = (member as MethodInfo);
                Assert.IsTrue(methodInfo.IsVirtual, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                Assert.IsTrue(propertyInfo.GetSetMethod().IsVirtual, propertyInfo.Name);
                Assert.IsTrue(propertyInfo.GetGetMethod().IsVirtual, propertyInfo.Name);
            }
        }
    }
    public static void EnsureMembersAreNotVirtual<T>(params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var member = typeof(T).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = (member as MethodInfo);
                Assert.IsFalse(methodInfo.IsVirtual, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.IsFalse(setMethod.IsVirtual, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.IsFalse(getMethod.IsVirtual, propertyInfo.Name);
            }
        }
    }

    public static void EnsureMembersAreSealed<T>(params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var member = typeof(T).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = (member as MethodInfo);
                Assert.IsTrue(methodInfo.IsFinal, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.IsTrue(setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.IsTrue(getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }
    public static void EnsureMembersAreNotSealed<T>(params string[] memberNames)
    {
        foreach (var memberName in memberNames)
        {
            var member = typeof(T).GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo)
            {
                var methodInfo = (member as MethodInfo);
                Assert.IsFalse(methodInfo.IsFinal, methodInfo.Name);
            }
            if (member is PropertyInfo)
            {
                var propertyInfo = member as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                Assert.IsFalse(setMethod.IsFinal, propertyInfo.Name);
                var getMethod = propertyInfo.GetGetMethod();
                Assert.IsFalse(getMethod.IsFinal, propertyInfo.Name);
            }
        }
    }


}