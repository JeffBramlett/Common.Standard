using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    /// <summary>
    /// Extending class for Ambassador
    /// </summary>
    class TestingAmbassador : AbstractAmbassador<string, int>
    {
        protected override int ExecuteOnEachInput(string input)
        {
            return input.Length;
        }
    }
}
