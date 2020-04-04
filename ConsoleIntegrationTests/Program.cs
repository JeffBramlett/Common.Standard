using Common.Standard;
using Common.Standard.Toggles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
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

            //TestFiltersAndPipe();

            //AmbassadorTest();

            TestGenericPool();
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

        private static void WaitForKey(string message = "")
        {
            string msg = string.IsNullOrEmpty(message) ? "Press any key to continue" : message;
            Console.WriteLine(msg);
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
            var step1a = new Step1Filter("Step 1a");
            var step2 = new Step2Filter();
            var step2a = new Step2Filter("Step 2a");
            var step3 = new Step3Filter();
            var step3a = new Step1Filter("Step 3a");

            step1.AddFilter(step1a);
            step2.AddFilter(step2a);
            step3.AddFilter(step3a);

            step1.FilterCompleted += Step1_FilterCompleted;
            step1.FilterExceptionEncountered += Step1_FilterExceptionEncountered;

            step1.AddFilter(step2);
            step1.AddFilter(step3);

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

        #region Ambassador Test

        private static void AmbassadorTest()
        {
            Console.WriteLine("Ambassador Test\n");

            TestingAmbassador amb = new TestingAmbassador();
            amb.InputCompleted += delegate(object sender, EventArgs args)
            {
                var completedArgs = args as AbstractAmbassador<string, int>.InputCompletedEventArgs;

                Console.WriteLine($"Input: {completedArgs.Input}\tResponse: {completedArgs.Response}\n");
            };

            amb.InputExceptionEncountered += delegate(object sender, EventArgs args)
            {
                var excepArgs = args as AbstractAmbassador<string, int>.InputExceptionEventArgs;

                Console.WriteLine($"Input: {excepArgs.Input}\tException: {excepArgs.Exception}\n");
            };

            amb.AddInputToQueue("This has 22 characters");

            Thread.Sleep(1000);

            WaitForKey();

            amb.Dispose();
        }
        #endregion

        #region Pool Test

        private static void TestGenericPool()
        {
            ConsoleWriter.Instance.WriteEmphasis("Generic Pool Test\n");

            IGenericObjectPool<TestPoolItem> testPool = new GenericObjectPool<TestPoolItem>(10, BalancingMethods.RoundRobin);

            List<TestPoolItem> items = new List<TestPoolItem>();
            for (var i = 0; i < 5; i++)
            {
                items.Add(testPool.AcquireItem().Result);
            }
            ConsoleWriter.Instance.WriteInformation($"Count: {testPool.Count}\tSize: {testPool.Size}");

            ConsoleWriter.Instance.WriteEmphasis("Press any key to acquire one more", true);

            var itemAdded = testPool.AcquireItem().Result;
            items.Add(itemAdded);
            ConsoleWriter.Instance.WriteInformation($"Count: {testPool.Count}\tSize: {testPool.Size}");

            ConsoleWriter.Instance.WriteEmphasis("Press any key to deactivate last added", true);
            itemAdded.Deactivate();

            ConsoleWriter.Instance.WriteInformation($"Count: {testPool.Count}\tSize: {testPool.Size}");

            ConsoleWriter.Instance.WriteEmphasis("Press any key to release all", true);

            for (var i = 0; i < items.Count; i++)
            {
                testPool.ReleaseItem(items[i]).Wait();
            }

            testPool.ContractItemPool();
            Thread.Sleep(10);

            ConsoleWriter.Instance.WriteInformation($"Count: {testPool.Count}\tSize: {testPool.Size}");

            ConsoleWriter.Instance.WriteEmphasis("Press any key to quit", true);

            testPool.Dispose();
            ConsoleWriter.Instance.Dispose();
        }
        #endregion
    }
}
