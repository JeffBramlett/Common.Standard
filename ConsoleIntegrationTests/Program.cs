using Common.Standard;
using Common.Standard.Toggles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.Extensions;
using Common.Standard.Generic;

namespace ConsoleIntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //SingletonTest();
            //DependencyTest();
            //ToggleTest();
            //TestCircuitBreakerDataType();
            //TestCircuitBreakerObj();

            TestFiltersAndPipe();

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

        private static void TestCircuitBreakerDataType()
        {
            Console.WriteLine("\nCircuit Breaker Test");

            using (CircuitBreaker cb = new CircuitBreaker(5, TimeSpan.FromSeconds(1)))
            {
                cb.BreakerStatusChange += Cb_BreakerStatusChange;

                int i = 0;

                var result = cb.Execute(i.ToString(), CbDelegate).Result;
                Console.WriteLine($"Circuit Break: {result}");

                cb.Reset().Wait();
            }

            WaitForKey();
        }

        private static int _test;

        private static int CbDelegate(string input)
        {
            int i = int.Parse(input);

            _test++;

            if (_test > 5)
                return _test;

            return 0;
        }

        private static void TestCircuitBreakerObj()
        {
            _test = 0;
            Console.WriteLine("\nCircuit Breaker Obj Test");

            CircuitBreakerObj cb = new CircuitBreakerObj(10, TimeSpan.FromSeconds(1));
            cb.BreakerStatusChange += Cb_BreakerStatusChange;

            ActuationObject obj = new ActuationObject(new AsyncTextWriter());

            var result = cb.Execute(obj, CBObjDelegate).Result;

            Console.WriteLine($"Circuit Break: {result}");

            WaitForKey();
        }

        private static void Cb_BreakerStatusChange(object sender, EventArgs e)
        {
            BreakerStatusChangedEventArgs bargs = e as BreakerStatusChangedEventArgs;
            Console.WriteLine($"Circuit Status Change: {bargs.BreakerStatus}");
        }

        private static int CBObjDelegate(ActuationObject input)
        {
            _test++;

            if (_test > 4)
                return _test;

            return 0;
        }

        private static void TestFiltersAndPipe()
        {
            Console.WriteLine("Filters and Piping Test\n");

            var step1 = new Step1Filter();
            var step2 = new Step2Filter();
            var step3 = new Step3Filter();

            step1.FilterCompleted += Step1_FilterCompleted;
            step1.FilterExceptionEncountered += Step1_FilterExceptionEncountered;

            step1.AddFilter(step2);
            step2.AddFilter(step3);

            step1.Pipe("Test");

            WaitForKey();
        }

        private static void Step1_FilterExceptionEncountered(object sender, EventArgs e)
        {
            var fea = e as FilterExceptionEventArgs;
            Console.WriteLine($"{fea.Exception.Message}");
        }

        private static void Step1_FilterCompleted(object sender, EventArgs e)
        {
            var cea = e as FilterCompleteEventArgs;
            Console.WriteLine($"{cea.Message}");
        }
    }
}
