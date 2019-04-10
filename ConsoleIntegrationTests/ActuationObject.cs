using Common.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleIntegrationTests
{
    class ActuationObject
    {
        private IAsyncTextWriter Writer { get; set; }

        public ActuationObject(IAsyncTextWriter writer)
        {
            Writer = writer;
        }

        public void Execute()
        {
            Writer.WriteLine("Execute called");
        }
    }
}
