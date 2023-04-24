using Virtuosity;

namespace ExcludeNamespace;

[DoNotVirtualize]
public class NotInNamespaceButWithAttribute
{
    public void Method()
    {
    }
}