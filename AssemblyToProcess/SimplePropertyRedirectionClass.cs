public class SimplePropertyRedirectionClass
{
    public string Property1 => Property2;

    public string Property2 { get; set; }
}