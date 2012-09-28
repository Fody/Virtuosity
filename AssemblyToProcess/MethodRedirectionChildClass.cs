public class MethodRedirectionChildClass : MethodRedirectionBaseClass
{
    public new string Method2()
    {
        return "Child";
    }
}