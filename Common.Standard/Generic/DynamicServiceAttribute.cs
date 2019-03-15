using System;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Attribute class to be applied to service classes that
    /// GenericServiceLoader can load.
    /// </summary>
    public sealed class DynamicServiceAttribute : Attribute
    {
        public string Title { get; set; }
        public string  Description { get; set; }      
    }
}
