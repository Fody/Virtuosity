using System.Linq;
using System.Reflection;

public static class VirtualTester
{
    public static void EnsureMembersAreVirtual(this Assembly assembly, string className, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Check.True(methodInfo.IsVirtual, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod != null)
                {
                    Check.True(setMethod.IsVirtual, propertyInfo.Name);
                }

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod != null)
                {
                    Check.True(getMethod.IsVirtual, propertyInfo.Name);
                }
            }
        }
    }

    public static void EnsureMembersAreNotVirtual(this Assembly assembly, string className, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Check.False(methodInfo.IsVirtual, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod != null)
                {
                    Check.False(setMethod.IsVirtual, propertyInfo.Name);
                }

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod != null)
                {
                    Check.False(getMethod.IsVirtual, propertyInfo.Name);
                }
            }
        }
    }

    public static void EnsureMembersAreSealed(this Assembly assembly, string className, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Check.True(methodInfo.IsFinal, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod != null)
                {
                    Check.True(setMethod.IsFinal, propertyInfo.Name);
                }

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod != null)
                {
                    Check.True(getMethod.IsFinal, propertyInfo.Name);
                }
            }
        }
    }

    public static void EnsureMembersAreNotSealed(this Assembly assembly, string className, params string[] memberNames)
    {
        var type = assembly.GetType(className, true);

        foreach (var memberName in memberNames)
        {
            var member = type.GetMember(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).First();
            if (member is MethodInfo methodInfo)
            {
                Check.False(methodInfo.IsFinal, methodInfo.Name);
            }

            if (member is PropertyInfo propertyInfo)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod != null)
                {
                    Check.False(setMethod.IsFinal, propertyInfo.Name);
                }

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod != null)
                {
                    Check.False(getMethod.IsFinal, propertyInfo.Name);
                }
            }
        }
    }
}