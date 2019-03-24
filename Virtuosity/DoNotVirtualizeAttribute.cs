using System;

namespace Virtuosity
{
    /// <summary>
    /// Used to exclude a class form virtualization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DoNotVirtualizeAttribute :
        Attribute
    {
    }
}