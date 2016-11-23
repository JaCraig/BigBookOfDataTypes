using Xunit;

namespace BigBook.Tests
{
    public class BagTests
    {
        [Fact]
        public void RandomTest()
        {
            var BagObject = new Bag<string>();
            var Rand = new System.Random();
            for (int x = 0; x < 10; ++x)
            {
                var Value = x.ToString();
                var Count = Rand.Next(1, 10);
                for (int y = 0; y < Count; ++y)
                    BagObject.Add(Value);
                Assert.Equal(Count, BagObject[Value]);
            }
            Assert.Equal(10, BagObject.Count);
        }
    }
}