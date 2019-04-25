using Common.Standard;
using Common.Standard.Toggles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleIntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            SingletonTest();
            DependencyTest();
            ToggleTest();
        }

        private static void ToggleTest()
        {
            Console.WriteLine("\nToggle Test");

            IToggleProvider toggleProv = new ToggleProvider(new DefaultToggleRepository());
            toggleProv.AddToggle("Test", DateTime.Now);

            Console.WriteLine("Toggle created");

            if (toggleProv.IsToggled("Test"))
            {
                Console.WriteLine("Toggle succeeded");
            }
            else
            {
                Console.WriteLine("Toggle failed");
            }

            WaitForKey();
        }

        private static void DependencyTest()
        {
            Console.WriteLine("\nAsyncWriter Dependency Test");

            AsyncTextWriter tw = new AsyncTextWriter();
            ActuationObject act = new ActuationObject(tw);

            act.Execute();

            WaitForKey();

            tw.Dispose();
        }

        private static void SingletonTest()
        {
            Console.WriteLine("\nAsyncWriter Singleton Test");

            AsyncTextWriter.Instance.Init("Integration Test.log");

            AsyncTextWriter.Instance.WriteLine("Test Line");
            AsyncTextWriter.Instance.Write("Part {0}", "A");
            AsyncTextWriter.Instance.Write("+{0}", "B");
            AsyncTextWriter.Instance.WriteLine("={0}", "C");
            AsyncTextWriter.Instance.WriteLine("Test complete");

            WaitForKey();

            AsyncTextWriter.Instance.Dispose();
        }

        private static void WaitForKey()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Write("\b \b");

        }
    }
}
