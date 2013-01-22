using System.IO;
using System.Reflection;
using Mono.Cecil;

public static class ModuleInitializer
{
    public static void Initialize()
    {
        WeaveAssembly("AssemblyWithIncludes.dll", "AssemblyWithIncludes.IncludeNamespace", null);
        WeaveAssembly("AssemblyWithExcludes.dll", null, "AssemblyWithExcludes.ExcludeNamespace");
        WeaveAssembly("AssemblyToProcess.dll", null, null);
    }

    static void WeaveAssembly(string fileName, string includeNamespaces, string excludeNamespaces)
    {
        var assemblyPath = Path.Combine(AssemblyLocation.CurrentDirectory(), fileName);

        var backupAssembly = assemblyPath.Replace(".dll", "_Backup.dll");
        File.Copy(assemblyPath, backupAssembly, true);

        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath);
        var weavingTask = new ModuleWeaver
            {
                IncludeNamespaces = includeNamespaces,
                ExcludeNamespaces = excludeNamespaces,
                ModuleDefinition = moduleDefinition
            };

        weavingTask.Execute();
        moduleDefinition.Write(assemblyPath);
        Assembly.LoadFrom(assemblyPath);
    }

}