using System;
using Mono.Cecil;

public partial class ModuleWeaver
{
    public Action<string> LogDebug { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }

    public ModuleWeaver()
    {
        LogDebug = s => { };
    }

    public void Execute()
    {
        ReadConfig();
        ProcessIncludesExcludes();
        ProcessAssembly();
        ConvertCallToCallVirtual();
        ConvertNewToOverrides();
        ConvertFunctionPointer();
    }
}