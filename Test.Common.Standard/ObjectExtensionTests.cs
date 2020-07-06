using System;
using System.Collections.Generic;
using System.Text;
using Common.Standard.Extensions;
using Xunit;

namespace Test.Common.Standard
{
    public class ObjectExtensionTests
    {

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

    }
}
