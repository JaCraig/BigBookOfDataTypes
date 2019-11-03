using Xunit;

namespace BigBook.Tests
{
    public class SetTests
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new Set<int>();
            var TestObject2 = new Set<int>();
            for (var x = 0; x < 10; ++x)
            {
                TestObject.Add(x);
            }

            for (var x = 9; x >= 0; --x)
            {
                TestObject2.Add(x);
            }

            Assert.True(TestObject.IsSubset(TestObject2));
            Assert.True(TestObject.Contains(TestObject2));
            Assert.True(TestObject.Intersect(TestObject2));
        }
    }
}