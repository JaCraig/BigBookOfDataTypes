using Xunit;

namespace BigBook.Tests
{
    public class FractionTests
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new BigBook.Fraction(9, 27);
            var TestObject2 = new BigBook.Fraction(3, 4);
            TestObject.Reduce();
            Assert.Equal(3, TestObject.Denominator);
            Assert.Equal(1, TestObject.Numerator);
            Assert.Equal(new BigBook.Fraction(1, 4), TestObject * TestObject2);
            Assert.Equal(new BigBook.Fraction(13, 12), TestObject + TestObject2);
            Assert.Equal(new BigBook.Fraction(-5, 12), TestObject - TestObject2);
            Assert.Equal(new BigBook.Fraction(4, 9), TestObject / TestObject2);
            Assert.Equal(new BigBook.Fraction(-1, 3), -TestObject);
            Assert.Equal(new BigBook.Fraction(9, 27), TestObject);
        }
    }
}