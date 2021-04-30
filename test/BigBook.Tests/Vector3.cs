using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests
{
    public class Vector3Tests : TestBaseClass<Vector3>
    {
        public Vector3Tests()
        {
            TestObject = new BigBook.Vector3(2.5, 4.1, 1.3);
        }

        [Fact]
        public void BasicTest()
        {
            var TestObject = new BigBook.Vector3(2.5, 4.1, 1.3);
            Assert.InRange(TestObject.Magnitude, 4.97, 4.98);
            TestObject.Normalize();
            Assert.InRange(TestObject.X, .5, .6);
            Assert.InRange(TestObject.Y, .82, .83);
            Assert.InRange(TestObject.Z, .26, .27);
        }
    }
}