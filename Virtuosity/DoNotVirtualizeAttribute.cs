using System;

namespace Virtuosity
{
    /// <summary>
    /// Used to exclude a class form vitualization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DoNotVirtualizeAttribute :
        Attribute
    {
    }
}