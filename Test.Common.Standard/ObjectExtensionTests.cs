using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Standard.Extensions;
using Xunit;

namespace Test.Common.Standard
{
    public class ObjectExtensionTests
    {
        /// <summary>
        /// Test getting the Assembly version
        /// </summary>
        [Fact]
        public void TestGaetAssemblyVersion()
        {
            var version = this.GetAssemblyVersion();

            Assert.Equal("1.0.0.0", version);
        }

        /// <summary>
        /// Test the CastAs extension
        /// </summary>
        [Fact]
        public void TestCastAs()
        {
            // make list object
            var asList = new List<string>()
            {
                "one",
                "two",
                "three"
            };

            // Cast list object to IEnumerable
            IEnumerable<string> asEnumerable;
            var success = asList.CastAs(out asEnumerable);

            // assert results
            Assert.True(success);
            Assert.Equal(asList.Count, asEnumerable.Count());
        }

        /// <summary>
        /// Test throwing null is object is null
        /// </summary>
        [Fact]
        public void TestThrowIfNull()
        {
            try
            {
                object isNullObject = null;

                isNullObject.ThrowIfNull();

                Assert.True(false);
            }
            catch (ArgumentNullException)
            {
                Assert.True(true);
            }
            catch(Exception)
            {
                Assert.True(false);
            }
        }

        /// <summary>
        /// Test serialization for data types
        /// </summary>
        [Fact]
        public void TestSerialization_DataTypes()
        {
            string testOfString = "string test";

            CaliforniaAreaCodes arrayTest = new CaliforniaAreaCodes();

            string arrayAsJson = arrayTest.ToJson();

            string asJson = testOfString.ToJson();
            string asXml = testOfString.ToXml();

            Assert.Equal("\"string test\"", asJson);
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<string>string test</string>", asXml);
        }

        /// <summary>
        /// Test serialization for Reference types
        /// </summary>
        [Fact]
        public void TestSerialization_RefTypes()
        {
            Something testOfRef = new Something()
            { Id = 2 };

            string asJson = testOfRef.ToJson();
            string asXml = testOfRef.ToXml();

            Assert.Equal("{\"Id\":2}", asJson);
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Something xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Id>2</Id>\r\n</Something>", asXml);
        }

        class CaliforniaAreaCodes
        {
            string _areaCodes;
            readonly string _message = "If the customer asks about CCPA (California Consumer Privacy Act), please refer to the matrices for instructions";

            public string AreaCodes
            {
                get
                {
                    if (string.IsNullOrEmpty(_areaCodes))
                    {
                        _areaCodes = "209, 213, 279, 310, 323, 408, 415, 424, 442, 510, 530, 559, 562, 619, 626, 628, 650, 657, 661, 669, 707, 714, 747, 760, 805, 818, 820, 831, 858, 909, 916, 925, 949, 951";
                    }

                    return _areaCodes;
                }
                set
                {
                    _areaCodes = value;
                }
            }

            public string Message
            {
                get
                {
                    return BuildMessage();
                }
            }

            private string BuildMessage()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<div style=\"font - family: Arial\">");
                sb.Append("<b>California Consumer Privacy Act (CCPA)</b> - the CCPA grants consumers the right to:");
                sb.Append("<ul>");
                sb.Append("< li>");
                sb.Append("Know what personal information (PI) a business collects, sells or discloses about them,");
                sb.Append("</li>");
                sb.Append("<li>");
                sb.Append("Receive notice before or at the point of collection about what PI categories a business collects and its intended use purposes");
                sb.Append("</li>");
                sb.Append("<li>");
                sb.Append("Access and in some cases receive a portable electronic version of their PI");
                sb.Append("</li>");
                sb.Append("<li>");
                sb.Append("Receive additional details regarding the PI that a business collects and its use purposes");
                sb.Append("</li>");
                sb.Append("<li>");
                sb.Append("Request that a business and its service providers delete their PI and,");
                sb.Append("</li>");
                sb.Append("< li>Opt-out of the sale of their PI.</li>");
                sb.Append("</ul>");
                sb.Append("<b>");
                sb.Append("Refer any caller asking about the CCPA or their Personal Information to <a href=\"www.agero.com/privacy\">www.agero.com/privacy</a> or to (833) 983-1112.");
                sb.Append("</b>");
                sb.Append("</div>");

                return sb.ToString();
            }
        }
    }
}
