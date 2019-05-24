using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    class TestingAmbassador : AbstractAmbassador<string, int>
    {
        protected override int ExecuteOnEachInput(string input)
        {
            return input.Length;
        }
    }
}
