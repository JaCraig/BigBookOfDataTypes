using BigBook.Tests.BaseClasses;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class ExpandoTypeConverterTests : TestingDirectoryFixture
    {
        [Fact]
        public void ConvertFrom()
        {
            IDictionary<string, object> Result = new TestClass { A = "This is a test", B = 10 }.To<TestClass, ExpandoObject>();
            Assert.Equal(10, Result["B"]);
            Assert.Equal("This is a test", Result["A"]);
        }

        [Fact]
        public void ConvertTo()
        {
            IDictionary<string, object> TestObject = new ExpandoObject();
            TestObject["A"] = "This is a test";
            TestObject["B"] = 10;
            var Result = TestObject.To<IDictionary<string, object>, TestClass>();
            Assert.Equal(10, Result.B);
            Assert.Equal("This is a test", Result.A);
        }

        public class TestClass
        {
            public string A { get; set; }

            public int B { get; set; }
        }
    }
}