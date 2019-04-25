using Common.Standard.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Test.Common.Standard
{
    public class SpoolerTests
    {
        [Fact]
        public void SpoolerEventExecutes_test()
        {
            string input = "test";
            string output = string.Empty;

            GenericSpooler<string> spooler = new GenericSpooler<string>();
            spooler.ItemSpooled += (item) => { output = item; };

            spooler.AddItem(input);

            Thread.Sleep(100);

            Assert.Equal(input, output);
        }

        [Fact]
        public void SpoolerEventExecutes_count_test()
        {
            int count = 0;
            int max = 10;

            GenericSpooler<string> spooler = new GenericSpooler<string>();
            spooler.ItemSpooled += (item) => { count++; };

            for (var i = 0; i < max; i++)
            {
                spooler.AddItem("Item" + i);
            }

            Thread.Sleep(100);

            Assert.Equal(max, count);
        }

        [Fact]
        public void SpoolerEventExecutes_Order_test()
        {
            int count = 0;
            List<int> spooledList = new List<int>();

            GenericSpooler<int> spooler = new GenericSpooler<int>();
            spooler.ItemSpooled += (item) => { spooledList.Add(item); };

            for (var i = 0; i < 10; i++)
            {
                spooler.AddItem(i);
            }

            Thread.Sleep(100);

            bool success = true;
            for (var i = 0; i < spooledList.Count - 1; i++)
            {
                var pos = spooledList[i];
                var next = spooledList[i + 1];

                var diff = next - pos;
                if (diff != 1)
                {
                    success = false;
                    break;
                }
            }

            Assert.True(success);
        }
    }
}
