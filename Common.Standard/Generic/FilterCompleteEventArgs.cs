using Common.Standard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Event args for Filter completion
    /// </summary>
    public class FilterCompleteEventArgs: EventArgs
    {
        /// <summary>
        /// The filter name
        /// </summary>
        public string FilterName { get; set; }

        /// <summary>
        /// Messages from the filter
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Downline filter complete Args
        /// </summary>
        public List<FilterCompleteEventArgs> MoreCompleteEventArgs { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="name">the name fo the filter</param>
        public FilterCompleteEventArgs(string name)
        {
            name.ThrowIfNull();

            FilterName = name;
            MoreCompleteEventArgs = new List<FilterCompleteEventArgs>();
        }
    }
}
