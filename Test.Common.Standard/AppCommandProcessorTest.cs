using Common.Standard.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Test.Common.Standard
{
    public class AppCommandProcessorTest
    {
        [Fact]
        public void TestCommandProcessor()
        {
            TestCommand cmd1 = new TestCommand();
            TestCommand cmd2 = new TestCommand();

            using(AppCommandProcessor proc = new AppCommandProcessor())
            {
                proc.Init("one", "two");

                var cmd1Added = proc.AddCommand(cmd1).Result;
                var cmd2Added = proc.AddCommand(cmd2).Result;

                Assert.True(cmd1Added);
                Assert.True(cmd2Added);

                // Wait to clear the spool before disposing
                Thread.Sleep(10);
            }

            Assert.True(cmd1.CanExecuteCalled);
            Assert.True(cmd1.ExecuteCalled);

            Assert.True(cmd2.CanExecuteCalled);
            Assert.True(cmd2.ExecuteCalled);
        }

        [Fact]
        public void TestCommandProcessorCommandSkip()
        {
            TestCommand cmd1 = new TestCommand();
            TestCommand cmd2 = new TestCommand();

            cmd2.NotExecutable = true;

            using (AppCommandProcessor proc = new AppCommandProcessor())
            {
                proc.Init("one", "two");

                var cmd1Added = proc.AddCommand(cmd1).Result;
                var cmd2Added = proc.AddCommand(cmd2).Result;

                Assert.True(cmd1Added);
                Assert.True(cmd2Added);

                // Wait to clear the spool before disposing
                Thread.Sleep(10);
            }

            Assert.True(cmd1.CanExecuteCalled);
            Assert.True(cmd1.ExecuteCalled);

            Assert.False(cmd2.CanExecuteCalled);
            Assert.False(cmd2.ExecuteCalled);
        }
    }

    #region Extending classes

    public class TestCommand : IAppCommand
    {
        public bool NotExecutable { get; set; }

        public bool CanExecuteCalled { get; set; }

        public bool ExecuteCalled { get; set; }


        public async Task<bool> CanExecute(params object[] args)
        {
            if (NotExecutable)
                return false;

            CanExecuteCalled = true;
            return await Task.FromResult(true);
        }

        public async Task Execute(params object[] args)
        {
            ExecuteCalled = true;

            await Task.FromResult(true);
        }
    }
    #endregion
}
