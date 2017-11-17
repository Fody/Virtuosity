using System.IO;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;

public class IntegrationTestsBase
{
    protected static Assembly assembly;

    protected static string beforeAssemblyPath;
    protected static string afterAssemblyPath;

    static IntegrationTestsBase()
    {
        beforeAssemblyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcess.dll");

        afterAssemblyPath = beforeAssemblyPath.Replace(".dll", "2.dll");
        File.Copy(beforeAssemblyPath, afterAssemblyPath, true);

        using (var moduleDefinition = ModuleDefinition.ReadModule(beforeAssemblyPath))
        {
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
            };

            weavingTask.Execute();
            moduleDefinition.Write(afterAssemblyPath);
        }

        assembly = Assembly.LoadFile(afterAssemblyPath);
    }
}