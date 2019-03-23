using Virtuosity;

namespace IncludeNamespace
{
    public class Outer
    {
        public class Inner
        {
            public string Property => "Bravo";
        }
        [DoNotVirtualize]
        public class InnerWithAttribute
        {
            public void Method()
            {
            }
        }
    }
}

namespace ExcludeNamespace
{
    public class Outer
    {
        public class Inner
        {
            public string Property => "Bravo";
        }
        [DoNotVirtualize]
        public class InnerWithAttribute
        {
            public void Method()
            {
            }
        }
    }
}