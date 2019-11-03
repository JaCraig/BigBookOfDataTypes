using Xunit;

namespace BigBook.Tests
{
    public class PriorityQueueTests
    {
        [Fact]
        public void RandomTest()
        {
            var TestObject = new PriorityQueue<int>();
            var Rand = new System.Random();
            var Value = 0;
            for (var x = 0; x < 10; ++x)
            {
                Value = Rand.Next();
                TestObject.Add(x, Value);
                Assert.Equal(Value, TestObject.Peek());
            }
            var HighestValue = TestObject.Peek();
            for (var x = 9; x >= 0; --x)
            {
                Value = Rand.Next();
                TestObject.Add(x, Value);
                Assert.Equal(HighestValue, TestObject.Peek());
            }
            var Count = 0;
            foreach (var Priority in TestObject.Keys)
            {
                foreach (var Item in TestObject[Priority])
                {
                    ++Count;
                }
            }
            Assert.Equal(20, Count);
        }
    }
}