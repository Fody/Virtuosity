public class PropertyRedirectionChildClass : PropertyRedirectionBaseClass
{
    public new string Property2
    {
        get { return "Child"; }
    }
}