using System;
using System.Collections.Generic;
using Xunit;
using Common.Standard.Extensions;
using Common.Standard.Generic;
using System.Threading;

namespace Test.Common.Standard
{
    public class ListExtensionsTests
    {
        [Fact]
        public void TestListExtension_TryFindItem()
        {
            List<string> inputList = new List<string>() { "1", "2", "3", "4", "5" };
            string foundInt;
            var foundSome = inputList.TryFindItem((x) => x == "3", out foundInt);

            Assert.True(foundSome);
        }

        [Fact]
        public void TestListExtension_TryFindValue()
        {
            List<int> inputList = new List<int>() { 1, 2, 3, 4, 5 };
            int foundInt;
            var foundSome = inputList.TryFindValue((x) => x == 3, out foundInt);

            Assert.True(foundSome);
        }


        [Fact]
        public void TestListExtension_TryFindAll()
        {
            List<int> inputList = new List<int>() { 1, 2, 3, 4, 5 };
            IList<int> greaterList;
            var foundSome = inputList.TryFindAll(out greaterList, (x) => x > 2);

            Assert.True(foundSome);
        }

        [Fact]
        public void TestListExtension_ToCommaDelimited_withQuotes()
        {
            List<int> inputList = new List<int>() {1, 2, 3, 4, 5};

            string delimited = inputList.ToCommaDelimited(true);

            Assert.Equal("\"1\",\"2\",\"3\",\"4\",\"5\"", delimited);
        }

        [Fact]
        public void TestListExtension_ToCommaDelimited_withoutQuotes()
        {
            List<int> inputList = new List<int>() { 1, 2, 3, 4, 5 };

            string delimited = inputList.ToCommaDelimited(false);

            Assert.Equal("1,2,3,4,5", delimited);
        }


        [Fact]
        public void TestListExtension_ApplyAction()
        {
            List<int> inputList = new List<int>() { 1, 2, 3, 4, 5 };

            int sum = 0;

            var result =  inputList.ApplyAction((x) => 
            {
                sum += x; 
            }, 1).Result;

            Assert.Equal(15, sum);
        }

        [Fact]
        public void TestListExtension_ApplyFunction()
        {
            List<int> inputList = new List<int>() { 1, 2, 3, 4, 5 };

            int sum = 0;

            var result = inputList.ApplyFunction((x) =>
            {
                return x * 2;
            }, 1).Result;

            Assert.Equal(2, result[0]);
            Assert.Equal(4, result[1]);
            Assert.Equal(6, result[2]);
            Assert.Equal(8, result[3]);
            Assert.Equal(10, result[4]);
        }


        [Fact]
        public void TestFirstReturn()
        {
            Func<int> f1 = () => { Thread.Sleep(10); return 5; };
            Func<int> f2 = () => { Thread.Sleep(20); return 10; };
            Func<int> f3 = () => {  return 15; };
            Func<int> f4 = () => { Thread.Sleep(30); return 20; };

            int r = Helpers.GetFirstValueReturned(f1, f2, f3, f4).Result;

            Assert.True(r == 15);
        }

        [Fact]
        public void TestFirstWitNullReturn()
        {
            Func<int> f1 = () => { return default(int); };
            Func<int> f2 = () => { return default(int); };
            Func<int> f3 = () => { return 15; };
            Func<int> f4 = () => { return default(int); };

            int r = Helpers.GetFirstValueReturned(f1, f2, f3, f4).Result;

            Assert.True(r == 15);
        }

        [Fact]
        public void TestFirstWitRefsReturn()
        {
            Func<Something> f1 = () => { return null; };
            Func<Something> f2 = () => { return null; };
            Func<Something> f3 = () => { return new Something() {Id = 15}; };
            Func<Something> f4 = () => { return null; };

            Something r = Helpers.GetFirstValueReturned(f1, f2, f3, f4).Result;

            Assert.True(r.Id == 15);
        }

        [Fact]
        public void TestExtensionFirstWitRefsReturn()
        {
            List<Func<Something>> funcList = new List<Func<Something>>()
            {
                new Func<Something>(() => { return null; }),
                new Func<Something> (() => { return null; }),
                new Func<Something>(() => { return new Something() { Id = 15 }; }),
                new Func<Something> (() => { return null; })
            };

            var r = funcList.ExecuteForFirstReturn().Result;
            Assert.True(r.Id == 15);
        }

        [Fact]
        public void TestExtensionFirstWithDataReturn()
        {
            List<Func<int>> funcList = new List<Func<int>>()
            {
                new Func<int>(() => { return 0; }),
                new Func<int> (() => { return 0; }),
                new Func<int>(() => { return  15; }),
                new Func<int> (() => { return 0; })
            };

            var r = funcList.ExecuteForFirstReturn().Result;
            Assert.True(r == 15);
        }

    }

    public class Something
    {
        public int Id { get; set; }
    }
}
