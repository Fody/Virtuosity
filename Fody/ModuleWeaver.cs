using System;
using Mono.Cecil;

public class ModuleWeaver
{
    public string IncludeNamespaces { get; set; }
    public string ExcludeNamespaces { get; set; }
    public Action<string> LogInfo { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }

    public ModuleWeaver()
    {
        LogInfo = s => { };
    }

    public void Execute()
    {
        var inclusionChecker = new InclusionChecker(this);
        inclusionChecker.Execute();
        var memberCache = new MemberCache();
        var typeProcessor = new TypeProcessor(this, memberCache);
        new AssemblyProcessor(typeProcessor, inclusionChecker, ModuleDefinition).Execute();
        new CallToCallVirtConverter(memberCache, ModuleDefinition).Execute();
        new NewToOverideConverter(memberCache, this, ModuleDefinition).Execute();
    }
}