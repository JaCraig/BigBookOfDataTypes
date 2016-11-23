using System.Collections.Concurrent;
using System.Linq;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ConcurrentDictionaryExtensionsTests
    {
        [Fact]
        public void CopyToTest()
        {
            var Test = new ConcurrentDictionary<string, int>();
            var Test2 = new ConcurrentDictionary<string, int>();
            Test.AddOrUpdate("Q", 4, (x, y) => 4);
            Test.AddOrUpdate("Z", 2, (x, y) => 2);
            Test.AddOrUpdate("C", 3, (x, y) => 3);
            Test.AddOrUpdate("A", 1, (x, y) => 1);
            Test.CopyTo(Test2);
            string Value = "";
            int Value2 = 0;
            foreach (string Key in Test2.Keys.OrderBy(x => x))
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
            Test.AddOrUpdate("Q", 4, (x, y) => 4);
            Test.AddOrUpdate("Z", 2, (x, y) => 4);
            Test.AddOrUpdate("C", 3, (x, y) => 4);
            Test.AddOrUpdate("A", 1, (x, y) => 4);
            Assert.Equal(4, Test.GetValue("Q"));
            Assert.Equal(0, Test.GetValue("V"));
            Assert.Equal(123, Test.GetValue("B", 123));
        }

        [Fact]
        public void SetValue()
        {
            var Test = new ConcurrentDictionary<string, int>();
            Test.AddOrUpdate("Q", 4, (x, y) => 4);
            Test.AddOrUpdate("Z", 2, (x, y) => 4);
            Test.AddOrUpdate("C", 3, (x, y) => 4);
            Test.AddOrUpdate("A", 1, (x, y) => 4);
            Assert.Equal(4, Test.GetValue("Q"));
            Test.SetValue("Q", 40);
            Assert.Equal(40, Test.GetValue("Q"));
        }
    }
}