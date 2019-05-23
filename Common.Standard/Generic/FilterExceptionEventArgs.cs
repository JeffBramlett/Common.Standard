using Common.Standard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    public class FilterExceptionEventArgs: EventArgs
    {
        public string FilterName { get; set; }

        public Exception Exception { get; set; }

        public List<FilterExceptionEventArgs> MoreExceptionEventArgs { get; set; }

        public FilterExceptionEventArgs(string name)
        {
            name.ThrowIfNull();

            MoreExceptionEventArgs = new List<FilterExceptionEventArgs>();
        }
    }
}
