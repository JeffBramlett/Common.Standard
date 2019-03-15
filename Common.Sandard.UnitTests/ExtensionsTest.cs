using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Sandard.Extensions;

namespace Common.Sandard.UnitTests
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void TestListExtension_ToCommaDelimited()
        {
            List<List<int>> inputList = new List<List<int>>()
            {
                new List<int>(){1,2,3,4,5},
                new List<int>(){11,22,33,44,55},
                new List<int>(){6,7,8,9,10}
            };

            string delimited = inputList.ToCommaDelimited()
        }
    }
}
