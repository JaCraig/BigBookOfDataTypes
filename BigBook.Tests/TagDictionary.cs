using BigBook;
using BigBook.Tests.BaseClasses;
using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class TagDictionaryTests : TestBaseClass<TagDictionary<string, string>>
    {
        public TagDictionaryTests()
        {
            TestObject = new TagDictionary<string, string>();
        }

        [Fact]
        public void BasicTest()
        {
            var TestObject = new TagDictionary<string, string>();
            10.Times(x => TestObject.Add("Object" + x, (x + 1).Times(y => "Key" + y).ToArray()));
            11.Times(x => Assert.Equal(10 - x, TestObject["Key" + x].Count()));
            Assert.Equal(10, TestObject["Key0"].Count());
            TestObject.Remove("Key0");
            Assert.Empty(TestObject["Key0"]);
            11.Times(x => Assert.Empty(TestObject["Key" + x]));
        }
    }
}