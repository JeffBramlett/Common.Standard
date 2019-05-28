using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    class TestPoolItem : IPoolItem
    {
        public bool IsActive { get; set; }

        public void Activate()
        {
            Console.WriteLine("Item Activated");
            IsActive = true;
        }

        public void Deactivate()
        {
            Console.WriteLine("Item Activated");
            IsActive = false;
        }

        public void Dispose()
        {
            Console.WriteLine("Item Disposed");

        }
    }
}
