using BigBook.Comparison;
using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.Comparison
{
    public class GenericEqualityComparerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Compare()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.True(Comparer.Equals("A", "A"));
            Assert.False(Comparer.Equals("A", "B"));
            Assert.False(Comparer.Equals("B", "A"));
        }

        [Fact]
        public void CompareNullNonValueType()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.True(Comparer.Equals(null, null));
            Assert.False(Comparer.Equals(null, "B"));
            Assert.False(Comparer.Equals("B", null));
        }

        [Fact]
        public void CompareValueType()
        {
            var Comparer = new GenericEqualityComparer<int>();
            Assert.True(Comparer.Equals(0, 0));
            Assert.False(Comparer.Equals(0, 1));
            Assert.False(Comparer.Equals(1, 0));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.Equal("A".GetHashCode(), Comparer.GetHashCode("A"));
            Assert.Equal("B".GetHashCode(), Comparer.GetHashCode("B"));
        }
    }
}