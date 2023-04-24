namespace Templates;

public class MethodRedirectionChildClass : MethodRedirectionBaseClass
{
    public override string Method2()
    {
        return "Child";
    }
}