using Common.Standard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Filter exception 
    /// </summary>
    public class FilterExceptionEventArgs: EventArgs
    {
        /// <summary>
        /// The filter name
        /// </summary>
        public string FilterName { get; set; }

        /// <summary>
        /// The exception that occurred in the filter
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The "sub" filters for this filter
        /// </summary>
        public List<FilterExceptionEventArgs> MoreExceptionEventArgs { get; set; }

        /// <summary>
        /// The default Ctor
        /// </summary>
        /// <param name="name"></param>
        public FilterExceptionEventArgs(string name)
        {
            name.ThrowIfNull();

            MoreExceptionEventArgs = new List<FilterExceptionEventArgs>();
        }
    }
}
