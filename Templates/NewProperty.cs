namespace Templates
{
    using System;

    public abstract class BasePropertyTemplate
    {
        public virtual object Value { get; set; }
    }

    public class DateTimeOffsetPropertyTemplate : BasePropertyTemplate
    {
        public new virtual DateTimeOffset Value
        {
            get => (DateTimeOffset) base.Value;
            set => base.Value = value;
        }
    }

    public class GenericPropertyTemplate<T> : BasePropertyTemplate
    {
        public new virtual T Value
        {
            get => (T) base.Value;
            set => base.Value = value;
        }
    }
}