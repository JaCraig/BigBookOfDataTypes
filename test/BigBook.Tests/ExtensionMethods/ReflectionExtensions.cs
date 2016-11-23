using BigBook.Tests.BaseClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public interface TestInterface
    {
        int Value { get; set; }

        int Value2 { get; set; }
    }

    public class ReflectionExtensionsTests : TestingDirectoryFixture
    {
        [Fact]
        public void CallMethodTest()
        {
            int Value = 10;
            Assert.Equal("10", Value.Call<string>("ToString"));
        }

        [Fact]
        public void CreateInstanceTest()
        {
            Assert.NotNull(typeof(TestClass).Create());
            Assert.IsType<TestClass>(typeof(TestClass).Create<TestInterface>());
        }

        [Fact]
        public void DumpPropertiesTest()
        {
            var TestObject = new List<int>();
            for (int x = 0; x < 10; ++x)
                TestObject.Add(x);
            Assert.Equal("<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody><tr><td>Capacity</td><td>16</td></tr><tr><td>Count</td><td>10</td></tr><tr><td>Item</td><td></td></tr></tbody></table>", TestObject.ToString(true));
        }

        [Fact]
        public void DumpPropertiesWithNoHtmlTest()
        {
            var TestObject = new List<int>();
            for (int x = 0; x < 10; ++x)
                TestObject.Add(x);

            string Output = "Property Name\t\t\t\tProperty Value" + Environment.NewLine +
                            "Capacity\t\t\t\t16" + Environment.NewLine +
                            "Count\t\t\t\t10" + Environment.NewLine +
                            "Item\t\t\t\t";

            Assert.Equal(Output, TestObject.ToString(false));
        }

        [Fact]
        public void GetAttribute()
        {
            var TestObject = typeof(TestClass).GetTypeInfo().Attribute<TestingAttribute>();
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void GetAttributes()
        {
            Assert.Equal(1, typeof(TestClass).GetTypeInfo().Attributes<TestingAttribute>().Length);
        }

        [Fact]
        public void GetNameTest()
        {
            Assert.Equal("BigBook.Tests.ExtensionMethods.TestClass", typeof(TestClass).GetName());
            Assert.Equal("BigBook.Tests.ExtensionMethods.TestClass2", typeof(TestClass2).GetName());
            Assert.Equal("BigBook.Tests.ExtensionMethods.TestClass3<System.Int32>", typeof(TestClass3<int>).GetName());
        }

        [Fact]
        public void GetPropertyGetterTest()
        {
            var TestObject = typeof(TestClass).GetProperty("Value").PropertyGetter<TestClass, int>();
            var TestObject2 = new TestClass();
            TestObject2.Value = 10;
            Assert.Equal(10, TestObject.Compile()(TestObject2));
        }

        [Fact]
        public void GetPropertyNameTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            Assert.Equal("Value", TestObject.PropertyName());
            Expression<Func<TestClass, int>> TestObject2 = x => x.Value2;
            Assert.Equal("Value2", TestObject2.PropertyName());
        }

        [Fact]
        public void GetPropertySetterTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            var TestObject2 = TestObject.PropertySetter<TestClass, int>();
            var TestObject3 = new TestClass();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Value);
        }

        [Fact]
        public void GetPropertySetterTest2()
        {
            Expression<Func<TestClass4, int>> TestObject = x => x.Temp.Value;
            var TestObject2 = TestObject.PropertySetter<TestClass4, int>();
            var TestObject3 = new TestClass4();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Temp.Value);
        }

        [Fact]
        public void GetPropertyTest()
        {
            Assert.Equal(1, new TestClass().Property("Value"));
            Assert.Equal(2, new TestClass().Property("Value2"));
        }

        [Fact]
        public void GetPropertyTypeTest()
        {
            Assert.Equal(typeof(int), new TestClass().PropertyType("Value"));
            Assert.Equal(typeof(int), new TestClass().PropertyType("Value2"));
        }

        [Fact]
        public void GetTypesTest()
        {
            Assert.Equal(3, typeof(ReflectionExtensionsTests)
                                        .GetTypeInfo()
                                        .Assembly
                                        .Types<TestInterface>()
                                        .Count());
        }

        [Fact]
        public void HasDefaultConstructor()
        {
            Assert.True(typeof(TestClass).HasDefaultConstructor());
        }

        [Fact]
        public void IsIEnumerableTest()
        {
            var TestObject = new List<int>();
            Assert.True(TestObject.GetType().Is(typeof(IEnumerable)));
        }

        [Fact]
        public void IsOfTypeTest()
        {
            var TestObject = new List<int>();
            Assert.True(TestObject.Is(typeof(List<int>)));
        }

        [Fact]
        public void MakeShallowCopyTest()
        {
            var TestObject1 = new TestClass();
            TestObject1.Value = 3;
            TestObject1.Value3 = "This is a test";
            var TestObject2 = TestObject1.MakeShallowCopy<TestClass>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
            Assert.Equal(TestObject1.Value3, TestObject2.Value3);
        }

        [Fact]
        public void MakeShallowCopyTest2()
        {
            var TestObject1 = new TestClass();
            TestObject1.Value = 3;
            var TestObject2 = TestObject1.MakeShallowCopy<TestInterface>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
        }

        [Fact]
        public void MarkedWith()
        {
            Assert.Equal(1, typeof(ReflectionExtensionsTests)
                                        .GetTypeInfo()
                                        .Assembly
                                        .Types<TestInterface>()
                                        .MarkedWith<TestingAttribute>()
                                        .Count());
        }

        [Fact]
        public void SetPropertyTest()
        {
            var TestObject1 = new TestClass();
            TestObject1.Property("Value", 3);
            Assert.Equal(3, TestObject1.Value);
        }

        [Fact]
        public void ToLongVersionString()
        {
            Assert.Equal("1.0.0.0", typeof(TestClass).GetTypeInfo().Assembly.ToString(VersionInfo.LongVersion));
        }

        [Fact]
        public void ToShortVersionString()
        {
            Assert.Equal("1.0", typeof(TestClass).GetTypeInfo().Assembly.ToString(VersionInfo.ShortVersion));
        }
    }

    [Testing]
    public class TestClass : TestInterface
    {
        public TestClass()
        {
            Value = 1; Value2 = 2;
        }

        public int Value { get; set; }

        public int Value2 { get; set; }

        public string Value3 = "ASDF";
    }

    public class TestClass2 : TestInterface
    {
        public int Value { get; set; }

        public int Value2 { get; set; }
    }

    public class TestClass3<T> : TestInterface
    {
        public int Value { get; set; }

        public int Value2 { get; set; }
    }

    public class TestClass4
    {
        public TestClass4()
        {
            Temp = new TestClass();
        }

        public TestClass Temp { get; set; }
    }

    public class TestingAttribute : Attribute
    {
    }
}