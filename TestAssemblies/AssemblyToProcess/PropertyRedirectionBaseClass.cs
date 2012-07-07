public class PropertyRedirectionBaseClass
{
    public string Property1
    {
        get { return Property2; }
    }

    public string Property2
    {
        get { return "Base"; }
    }
}