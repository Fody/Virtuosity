namespace Templates
{
    public class SimpleMethodRedirectionClass
    {
        public virtual string Method1()
        {
            return Method2();
        }
        public virtual string Method2()
        {
            return "Base";
        }
    }
}