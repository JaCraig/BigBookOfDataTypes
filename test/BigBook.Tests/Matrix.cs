using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests
{
    public class MatrixTests : TestingDirectoryFixture
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new BigBook.Matrix(3, 3, new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            var TestObject2 = new BigBook.Matrix(3, 3, new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            Assert.Equal(4, TestObject[1, 0]);
            Assert.Equal(8, (TestObject + TestObject2)[1, 0]);
            Assert.Equal(8, (TestObject * 2)[1, 0]);
        }
    }
}