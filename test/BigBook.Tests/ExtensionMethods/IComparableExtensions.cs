using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class IComparableExtensionsTests : TestingDirectoryFixture
    {
        [Fact]
        public void BetweenTest()
        {
            const int Value = 1;
            Assert.True(Value.Between(0, 2));
            Assert.False(Value.Between(2, 10));
        }

        [Fact]
        public void ClampTest()
        {
            const int Value = 10;
            Assert.Equal(9, Value.Clamp(9, 1));
            Assert.Equal(11, Value.Clamp(15, 11));
            Assert.Equal(10, Value.Clamp(11, 1));
        }

        [Fact]
        public void MaxTest()
        {
            const int Value = 4;
            Assert.Equal(5, Value.Max(5));
            Assert.Equal(4, Value.Max(1));
        }

        [Fact]
        public void MinTest()
        {
            const int Value = 4;
            Assert.Equal(4, Value.Min(5));
            Assert.Equal(1, Value.Min(1));
        }
    }
}