using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class ManyToManyIndexTests
    {
        [Fact]
        public void Add()
        {
            var TestObject = new ManyToManyIndex<int, string>();
            TestObject.Add(1, "A", "B");
            Assert.True(TestObject.TryGetValue(1, out var Values));
            Assert.Equal(2, Values.Count());
            Assert.True(TestObject.TryGetValue("A", out var ValuesInt));
            Assert.Equal(1, ValuesInt.Count());
        }

        [Fact]
        public void Remove()
        {
            var TestObject = new ManyToManyIndex<int, string>();
            TestObject.Add(1, "A", "B");
            Assert.True(TestObject.TryGetValue(1, out var Values));
            Assert.Equal(2, Values.Count());
            Assert.True(TestObject.TryGetValue("A", out var ValuesInt));
            Assert.Equal(1, ValuesInt.Count());
            TestObject.Remove("B");
            Assert.True(TestObject.TryGetValue(1, out Values));
            Assert.Equal(1, Values.Count());
            Assert.True(TestObject.TryGetValue("A", out ValuesInt));
            Assert.Equal(1, ValuesInt.Count());
            Assert.False(TestObject.TryGetValue("B", out ValuesInt));
            Assert.Equal(0, ValuesInt.Count());
            TestObject.Remove("A");
            Assert.False(TestObject.TryGetValue(1, out Values));
            Assert.Equal(0, Values.Count());
            Assert.False(TestObject.TryGetValue("A", out ValuesInt));
            Assert.Equal(0, ValuesInt.Count());
            Assert.False(TestObject.TryGetValue("B", out ValuesInt));
            Assert.Equal(0, ValuesInt.Count());
        }
    }
}