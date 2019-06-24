using System;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Attribute class to be applied to service classes that
    /// GenericServiceLoader can load.
    /// </summary>
    public sealed class DynamicServiceAttribute : Attribute
    {
        /// <summary>
        /// The service title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The service description
        /// </summary>
        public string  Description { get; set; }      
    }
}
