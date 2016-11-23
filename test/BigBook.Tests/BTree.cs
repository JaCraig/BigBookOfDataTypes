using Xunit;

namespace BigBook.Tests
{
    public class BTreeTests
    {
        [Fact]
        public void Creation()
        {
            var Tree = new BinaryTree<int>();
            Tree.Add(1);
            Tree.Add(2);
            Tree.Add(0);
            Tree.Add(-1);
            Assert.Equal(-1, Tree.MinValue);
            Assert.Equal(2, Tree.MaxValue);
        }

        [Fact]
        public void Random()
        {
            var Tree = new BinaryTree<int>();
            var Values = new System.Collections.Generic.List<int>();
            var Rand = new System.Random();
            for (int x = 0; x < 10; ++x)
            {
                var Value = Rand.Next();
                Values.Add(Value);
                Tree.Add(Value);
            }
            for (int x = 0; x < 10; ++x)
            {
                Assert.Contains(Values[x], Tree);
            }
            Values.Sort();
            Assert.Equal(Values.ToString(x => x.ToString(), " "), Tree.ToString());
        }
    }
}