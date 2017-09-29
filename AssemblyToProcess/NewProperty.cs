namespace NewProperty
{
    using System;

    public abstract class BaseProperty
    {
        public object Value { get; set; }

        public object ReplacedValue { get; set; }
    }

    public class DateTimeOffsetProperty : BaseProperty
    {
        public new DateTimeOffset Value
        {
            get { return (DateTimeOffset)base.Value; }
            set { base.Value = value; }
        }

        public new DateTimeOffset ReplacedValue { get; set; }
    }

    public class GenericProperty<T> : BaseProperty
    {
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        public new T ReplacedValue { get; set; }
    }

    public class BasePropertyUser
    {
        readonly BaseProperty property;

        public BasePropertyUser(BaseProperty property)
        {
            this.property = property;
        }

        public void Foo()
        {
            property.Value = "test1";
            property.ReplacedValue = "test2";
            Console.WriteLine($"{property.Value} and {property.ReplacedValue}");
        }
    }
}