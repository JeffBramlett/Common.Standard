using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    public class CircuitBreaker: AbstractCircuitBreaker<string>
    {
        public CircuitBreaker(int count, TimeSpan retryTimeSpan)
        {
            Threshold = count;
            RetryTimeSpan = retryTimeSpan;
        }

    }
}
