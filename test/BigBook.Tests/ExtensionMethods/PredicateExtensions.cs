using System;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    /// <summary>
    /// Predicate extensions
    /// </summary>
    public class PredicateExtensionsTests
    {
        [Fact]
        public void AddToSet()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Even = Even.AddToSet(1, 3, 5);
            Assert.True(Even(1));
            Assert.True(Even(3));
            Assert.True(Even(2));
            Assert.True(Even(10));
            Assert.True(Even(5));
        }

        [Fact]
        public void CartesianProduct()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            var CartesianProductResult = Even.CartesianProduct(Multiple3);
            Assert.True(CartesianProductResult(6, 12));
            Assert.False(CartesianProductResult(3, 9));
        }

        [Fact]
        public void Difference()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            var Diff = Even.Difference(Multiple3);
            Assert.True(Diff(2));
            Assert.True(Diff(4));
            Assert.False(Diff(6));
        }

        [Fact]
        public void Intersect()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            var Inter = Even.Intersect(Multiple3);
            Assert.True(Inter(6));
            Assert.True(Inter(12));
            Assert.False(Inter(2));
            Assert.False(Inter(3));
        }

        [Fact]
        public void RelativeComplement()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            var Compliement = Even.RelativeComplement(Multiple3);
            Assert.True(Compliement(2));
            Assert.False(Compliement(6));
        }

        [Fact]
        public void RemoveFromSet()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Even = Even.RemoveFromSet(2, 4, 6);
            Assert.True(Even(10));
            Assert.True(Even(8));
            Assert.False(Even(6));
            Assert.False(Even(4));
            Assert.False(Even(2));
        }

        [Fact]
        public void Union()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            var Test = Even.Union(Multiple3);
            Assert.True(Test(2));
            Assert.True(Test(3));
            Assert.True(Test(4));
            Assert.False(Test(5));
        }
    }
}