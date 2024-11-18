using BigBook.Comparison;
using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.Comparison
{
    public class GenericComparerTests : TestBaseClass<GenericComparer<string>>
    {
        public GenericComparerTests()
        {
            TestObject = new GenericComparer<string>();
        }

        [Fact]
        public void Compare()
        {
            var Comparer = new GenericComparer<string>();
            Assert.Equal(0, Comparer.Compare("A", "A"));
            Assert.Equal(-1, Comparer.Compare("A", "B"));
            Assert.Equal(1, Comparer.Compare("B", "A"));
        }

        [Fact]
        public void CompareNullNonValueType()
        {
            var Comparer = new GenericComparer<string>();
            Assert.Equal(0, Comparer.Compare(null, null));
            Assert.Equal(-1, Comparer.Compare(null, "B"));
            Assert.Equal(-1, Comparer.Compare("B", null));
        }

        [Fact]
        public void CompareValueType()
        {
            var Comparer = new GenericComparer<int>();
            Assert.Equal(0, Comparer.Compare(0, 0));
            Assert.Equal(-1, Comparer.Compare(0, 1));
            Assert.Equal(1, Comparer.Compare(1, 0));
        }
    }
}