using Common.Standard;
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
        }

        private static void DependencyTest()
        {
            AsyncTextWriter tw = new AsyncTextWriter();
            ActuationObject act = new ActuationObject(tw);

            act.Execute();

            Console.WriteLine("Dependency Test, Press any key to continue");
            Console.ReadKey();
            Console.Write("\b \b");

            tw.Dispose();
        }

        private static void SingletonTest()
        {
            AsyncTextWriter.Instance.Init("Integration Test.log");

            AsyncTextWriter.Instance.WriteLine("Test Line");
            AsyncTextWriter.Instance.Write("Part {0}", "A");
            AsyncTextWriter.Instance.Write("+{0}", "B");
            AsyncTextWriter.Instance.WriteLine("={0}", "C");
            AsyncTextWriter.Instance.WriteLine("Test complete");

            Console.WriteLine("Singleton Test, Press any key to continue");
            Console.ReadKey();
            Console.Write("\b \b");

            AsyncTextWriter.Instance.Dispose();
        }
    }
}
