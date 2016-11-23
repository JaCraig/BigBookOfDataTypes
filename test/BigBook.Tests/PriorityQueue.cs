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
            int Value = 0;
            for (int x = 0; x < 10; ++x)
            {
                Value = Rand.Next();
                TestObject.Add(x, Value);
                Assert.Equal(Value, TestObject.Peek());
            }
            var HighestValue = TestObject.Peek();
            for (int x = 9; x >= 0; --x)
            {
                Value = Rand.Next();
                TestObject.Add(x, Value);
                Assert.Equal(HighestValue, TestObject.Peek());
            }
            int Count = 0;
            foreach (int Priority in TestObject.Keys)
            {
                foreach (int Item in TestObject[Priority])
                {
                    ++Count;
                }
            }
            Assert.Equal(20, Count);
        }
    }
}