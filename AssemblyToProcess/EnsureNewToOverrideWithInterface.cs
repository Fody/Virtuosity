
namespace EnsureNewToOverrideWithInterface
{
    public interface IInterface
    {
        string Property { get; }
    }

    public class BaseImplementation : IInterface
    {
        public string Property
        {
            get { return "Alpha"; }
        }
    }

    public class ChildImplementation : BaseImplementation
    {
        public string Property
        {
            get { return "Bravo"; }
        }
    }
}