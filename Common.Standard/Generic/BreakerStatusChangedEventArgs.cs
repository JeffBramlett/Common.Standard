using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// EventArgs for Breaker status change
    /// </summary>
    public class BreakerStatusChangedEventArgs: EventArgs
    {
        /// <summary>
        /// The status change for Circuit breaker
        /// </summary>
        public CircuitBreakerStatuses BreakerStatus { get; set; }
    }
}
