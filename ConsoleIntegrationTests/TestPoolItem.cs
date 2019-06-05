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

        public event EventHandler ItemIsDeactivated;

        public TestPoolItem()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            ItemIsDeactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Console.WriteLine("Item Disposed");

        }
    }
}
