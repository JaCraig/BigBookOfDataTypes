using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ArrayExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType => typeof(ArrayExtensions);

        [Fact]
        public void ClearGenericTest()
        {
            int[] TestObject = { 1, 2, 3, 4, 5, 6 };
            TestObject = TestObject.Clear();
            foreach (var Item in TestObject)
            {
                Assert.Equal(0, Item);
            }
        }

        [Fact]
        public void ClearNullTest()
        {
            int[] TestObject = null;
            TestObject = TestObject.Clear();
            Assert.Null(TestObject);
        }

        [Fact]
        public void ClearTest()
        {
            var TestObject = new int[] { 1, 2, 3, 4, 5, 6 };
            TestObject = TestObject.Clear();
            foreach (var Item in TestObject)
            {
                Assert.Equal(0, Item);
            }
        }

        [Fact]
        public void CombineTest()
        {
            int[] TestObject1 = { 1, 2, 3 };
            int[] TestObject2 = { 4, 5, 6 };
            int[] TestObject3 = { 7, 8, 9 };
            TestObject1 = TestObject1.Concat(TestObject2, TestObject3);
            for (var x = 0; x < 8; ++x)
            {
                Assert.Equal(x + 1, TestObject1[x]);
            }
        }

        [Fact]
        public void ConcatNull2Test()
        {
            int[] TestObject1 = { 1, 2, 3 };
            TestObject1 = TestObject1.Concat(null);
            for (var x = 0; x < 2; ++x)
            {
                Assert.Equal(x + 1, TestObject1[x]);
            }
        }

        [Fact]
        public void ConcatNullTest()
        {
            int[] TestObject1 = null;
            int[] TestObject2 = { 4, 5, 6 };
            int[] TestObject3 = { 7, 8, 9 };
            TestObject1 = TestObject1.Concat(TestObject2, TestObject3);
            for (var x = 3; x < 8; ++x)
            {
                Assert.Equal(x + 1, TestObject1[x - 3]);
            }
        }
    }
}