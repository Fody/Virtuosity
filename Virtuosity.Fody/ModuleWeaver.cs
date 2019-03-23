using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

public partial class ModuleWeaver:BaseModuleWeaver
{
    List<TypeDefinition> allClasses;

    public override void Execute()
    {
        allClasses = ModuleDefinition.GetAllClasses();
        ReadConfig();
        ProcessIncludesExcludes();
        ProcessAssembly();
        ConvertCallToCallVirtual();
        ConvertNewToOverrides();
        ConvertFunctionPointer();
        CleanAttributes();
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        return Enumerable.Empty<string>();
    }

    public override bool ShouldCleanReference => true;
}