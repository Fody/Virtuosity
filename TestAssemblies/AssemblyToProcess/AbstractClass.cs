public abstract class AbstractClass
{
    public abstract string Property1 { get; set; }
    public abstract string Property2 { get; set; }
    public string NonAbstractProperty { get; set; }
    public abstract string Method1();
    public abstract string Method2();
	public string NonAbstractMethod()
	{
		return "foo";
	}
}