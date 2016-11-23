using BigBook.Comparison;
using Xunit;

namespace BigBook.Tests.Comparison
{
    public class SimpleComparerTests
    {
        [Fact]
        public void Compare()
        {
            var Comparer = new SimpleComparer<string>((x, y) => string.Compare(x, y, System.StringComparison.Ordinal));
            Assert.Equal(0, Comparer.Compare("A", "A"));
            Assert.Equal(-1, Comparer.Compare("A", "B"));
            Assert.Equal(1, Comparer.Compare("B", "A"));
        }
    }
}