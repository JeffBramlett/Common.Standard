using Common.Standard.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    public class FilterCompleteEventArgs: EventArgs
    {
        public string FilterName { get; set; }

        public string Message { get; set; }

        public List<FilterCompleteEventArgs> MoreCompleteEventArgs { get; set; }

        public FilterCompleteEventArgs(string name)
        {
            name.ThrowIfNull();

            FilterName = name;
            MoreCompleteEventArgs = new List<FilterCompleteEventArgs>();
        }
    }
}
