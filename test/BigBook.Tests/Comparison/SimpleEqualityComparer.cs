using BigBook.Comparison;
using Xunit;

namespace BigBook.Tests.Comparison
{
    public class SimpleEqualityComparerTests
    {
        [Fact]
        public void Compare()
        {
            var Comparer = new SimpleEqualityComparer<string>((x, y) => string.Equals(x, y), x => x.GetHashCode());
            Assert.True(Comparer.Equals("A", "A"));
            Assert.False(Comparer.Equals("A", "B"));
            Assert.False(Comparer.Equals("B", "A"));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var Comparer = new SimpleEqualityComparer<string>((x, y) => string.Equals(x, y), x => x.GetHashCode());
            Assert.Equal("A".GetHashCode(), Comparer.GetHashCode("A"));
            Assert.Equal("B".GetHashCode(), Comparer.GetHashCode("B"));
        }
    }
}