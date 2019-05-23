using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    class Step1Filter : AbstractFilterAndPipe<string>
    {
        public override bool CanFilterItem(string item)
        {
            return true;
        }

        public override void Filter(string item)
        {
            Console.WriteLine($"Step 1 Piped: {item}");
        }

        public Step1Filter():
            base("Step 1")
        {
            
        }
    }

    class Step2Filter: AbstractFilterAndPipe<string>
    {
        public override bool CanFilterItem(string item)
        {
            return true;
        }

        public override void Filter(string item)
        {
            Console.WriteLine($"Step 2 Piped: {item}");
            throw new NotImplementedException();
        }

        public Step2Filter() :
            base("Step 2")
        {

        }
    }

    class Step3Filter : AbstractFilterAndPipe<string>
    {
        public override bool CanFilterItem(string item)
        {
            return true;
        }

        public override void Filter(string item)
        {
            Console.WriteLine($"Step 3 Piped: {item}");
        }

        public Step3Filter() :
            base("Step 3")
        {

        }
    }
}
