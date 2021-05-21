using BigBook.Tests.BaseClasses;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class ExpandoTypeConverterTests : TestBaseClass<ExpandoObject>
    {
        public ExpandoTypeConverterTests()
        {
            TestObject = new ExpandoObject();
        }

        [Fact]
        public void ConvertFrom()
        {
            IDictionary<string, object> Result = new TestClass { A = "This is a test", B = 10, C = new Uri("http://B") }.To<TestClass, ExpandoObject>();
            Assert.Equal(10, Result["B"]);
            Assert.Equal("This is a test", Result["A"]);
            Assert.Equal(new Uri("http://B"), Result["C"]);
        }

        [Fact]
        public void ConvertTo()
        {
            IDictionary<string, object> TestObject = new ExpandoObject();
            TestObject["A"] = "This is a test";
            TestObject["B"] = 10;
            TestObject["C"] = "http://a";
            var Result = TestObject.To<IDictionary<string, object>, TestClass>();
            Assert.Equal(10, Result.B);
            Assert.Equal("This is a test", Result.A);
            Assert.Equal("http://a/", Result.C.ToString());
        }

        public class TestClass
        {
            public string A { get; set; }

            public int B { get; set; }

            public Uri C { get; set; }
        }
    }
}