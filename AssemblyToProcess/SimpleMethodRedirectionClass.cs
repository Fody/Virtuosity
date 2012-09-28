public class SimpleMethodRedirectionClass
{
    public string Method1()
    {
        return Method2();
    }

    public string Method2()
    {
        return "Base";
    }
}