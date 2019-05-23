using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    class CircuitBreakerObj: AbstractCircuitBreaker<ActuationObject>
    {
        public CircuitBreakerObj(int count, TimeSpan retry)
        {
            Threshold = count;
            RetryTimeSpan = retry;
        }
    }
}
