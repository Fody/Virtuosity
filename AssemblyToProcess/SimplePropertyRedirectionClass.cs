public class SimplePropertyRedirectionClass
{
    public string Property1
    {
        get { return Property2; }
    }

    public string Property2 { get; set; }

}