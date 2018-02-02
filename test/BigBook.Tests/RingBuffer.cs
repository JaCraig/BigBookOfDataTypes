using BigBook;
using System;
using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class RingBufferTests
    {
        [Fact]
        public void RandomTest()
        {
            var TestObject = new RingBuffer<int>(10);
            var Rand = new System.Random();
            int Value = 0;
            for (int x = 0; x < 10; ++x)
            {
                Value = Rand.Next();
                TestObject.Add(Value);
                Assert.Single(TestObject);
                Assert.Equal(Value, TestObject.Remove());
            }
            Assert.Empty(TestObject);
            var Values = new System.Collections.Generic.List<int>();
            for (int x = 0; x < 10; ++x)
            {
                Values.Add(Rand.Next());
            }
            TestObject.Add(Values);
            Assert.Equal(Values.ToArray(), TestObject.ToArray());

            for (int x = 0; x < 10; ++x)
            {
                Assert.Throws<InvalidOperationException>(() => TestObject.Add(Rand.Next()));
                Assert.Equal(10, TestObject.Count);
            }
        }
    }
}