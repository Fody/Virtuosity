
namespace EnsureNewToOverrideWithInterface;

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
#pragma warning disable 108, 114
    public string Property => "Bravo";
#pragma warning restore 108, 114
}