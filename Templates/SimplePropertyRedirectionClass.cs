namespace Templates
{
    public class SimplePropertyRedirectionClass
    {
        public virtual string Property1
        {
            get
            {
                return Property2;
            }
        }

        public virtual string Property2 { get; set; }

    }
}