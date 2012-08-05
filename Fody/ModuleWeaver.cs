using System;
using System.Linq;
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
        var allTypes = ModuleDefinition.GetTypes().ToList();
        new AssemblyProcessor(typeProcessor, inclusionChecker, allTypes).Execute();
        new CallToCallVirtConverter(memberCache, allTypes).Execute();
        new NewToOverideConverter(memberCache, allTypes).Execute();
    }
}