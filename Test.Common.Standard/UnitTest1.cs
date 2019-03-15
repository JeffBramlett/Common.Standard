using System;
using System.Collections.Generic;
using Xunit;
using   Common.Standard.Extensions;

namespace Test.Common.Standard
{
    public class UnitTest1
    {
        [Fact]
        public void TestListExtension_ToCommaDelimited()
        {
            List<int> inputList = new List<int>() {1, 2, 3, 4, 5};

            string delimited = inputList.ToCommaDelimited(true);
        }
    }
}
