using System.Collections.Generic;
using System.Linq;
using Fody;

public partial class ModuleWeaver:BaseModuleWeaver
{
    public override void Execute()
    {
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