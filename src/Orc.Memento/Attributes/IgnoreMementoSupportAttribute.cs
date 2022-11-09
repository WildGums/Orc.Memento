namespace Orc.Memento
{
    using System;

    /// <summary>
    /// This attribute prevents the specified property or method to be monitored by the <see cref="IMementoService"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class IgnoreMementoSupportAttribute : Attribute
    {
    }
}
