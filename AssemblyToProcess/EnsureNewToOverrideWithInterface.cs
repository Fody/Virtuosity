
namespace EnsureNewToOverrideWithInterface
{
    public interface IInterface
    {
        string Property { get; }
    }

    public class BaseImplementation : IInterface
    {
        public string Property => "Alpha";
    }

    public class ChildImplementation : BaseImplementation
    {
        public string Property => "Bravo";
    }
}