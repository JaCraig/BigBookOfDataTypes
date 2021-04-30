using BigBook.Tests.BaseClasses;
using System.Collections.Concurrent;
using System.Linq;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ConcurrentDictionaryExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType => typeof(ConcurrentDictionaryExtensions);

        [Fact]
        public void CopyToTest()
        {
            var Test = new ConcurrentDictionary<string, int>();
            var Test2 = new ConcurrentDictionary<string, int>();
            Test.AddOrUpdate("Q", 4, (_, __) => 4);
            Test.AddOrUpdate("Z", 2, (_, __) => 2);
            Test.AddOrUpdate("C", 3, (_, __) => 3);
            Test.AddOrUpdate("A", 1, (_, __) => 1);
            Test.CopyTo(Test2);
            var Value = "";
            var Value2 = 0;
            foreach (var Key in Test2.Keys.OrderBy(x => x))
            {
                Value += Key;
                Value2 += Test2[Key];
            }
            Assert.Equal("ACQZ", Value);
            Assert.Equal(10, Value2);
        }

        [Fact]
        public void GetValue()
        {
            var Test = new ConcurrentDictionary<string, int>();
            Test.AddOrUpdate("Q", 4, (_, __) => 4);
            Test.AddOrUpdate("Z", 2, (_, __) => 4);
            Test.AddOrUpdate("C", 3, (_, __) => 4);
            Test.AddOrUpdate("A", 1, (_, __) => 4);
            Assert.Equal(4, Test.GetValue("Q"));
            Assert.Equal(0, Test.GetValue("V"));
            Assert.Equal(123, Test.GetValue("B", 123));
        }

        [Fact]
        public void SetValue()
        {
            var Test = new ConcurrentDictionary<string, int>();
            Test.AddOrUpdate("Q", 4, (_, __) => 4);
            Test.AddOrUpdate("Z", 2, (_, __) => 4);
            Test.AddOrUpdate("C", 3, (_, __) => 4);
            Test.AddOrUpdate("A", 1, (_, __) => 4);
            Assert.Equal(4, Test.GetValue("Q"));
            Test.SetValue("Q", 40);
            Assert.Equal(40, Test.GetValue("Q"));
        }
    }
}