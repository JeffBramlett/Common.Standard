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
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int ActiveCount { get; set; }

        public int MaxCount { get; set; }

        public event EventHandler ItemIsDeactivated;

        public TestPoolItem()
        {
            IsActive = false;
        }

        public void Activate(params object[] startupObjects)
        {
            IsActive = true;
            ActiveCount++;
        }

        public void Deactivate()
        {
            IsActive = false;
            ItemIsDeactivated?.Invoke(this, EventArgs.Empty);
            ActiveCount--;
        }

        public void Dispose()
        {
            Console.WriteLine("Item Disposed");

        }
    }
}
