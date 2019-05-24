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
            Console.WriteLine($"{Name} Piped: {item}");
        }

        public Step1Filter():
            base("Step 1")
        {
            
        }

        public Step1Filter(string name):
            base(name)
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
            Console.WriteLine($"{Name} Piped: {item}");
            throw new NotImplementedException();
        }

        public Step2Filter() :
            base("Step 2")
        {

        }

        public Step2Filter(string name) :
            base(name)
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
            Console.WriteLine($"{Name} Piped: {item}");
        }

        public Step3Filter() :
            base("Step 3")
        {

        }

        public Step3Filter(string name) :
            base(name)
        {
        }
    }
}
