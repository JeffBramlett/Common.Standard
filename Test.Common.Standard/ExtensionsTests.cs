using System;
using System.Collections.Generic;
using Xunit;
using   Common.Standard.Extensions;
using Common.Standard.Generic;
using System.Threading;

namespace Test.Common.Standard
{
    public class ExtensionsTests
    {
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

        [Fact]
        public void TestSerialization_DataTypes()
        {
            string testOfString = "string test";

            string asJson = testOfString.ToJson();
            string asXml = testOfString.ToXml();
        }

        [Fact]
        public void TestSerialization_RefTypes()
        {
            Something testOfRef = new Something()
            { Id = 2 };

            string asJson = testOfRef.ToJson();
            string asXml = testOfRef.ToXml();
        }

        public class Something
        {
            public int Id { get; set; }
        }
    }
}
