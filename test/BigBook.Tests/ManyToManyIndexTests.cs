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
            Assert.Single(ValuesInt);
        }

        [Fact]
        public void Remove()
        {
            var TestObject = new ManyToManyIndex<int, string>();
            TestObject.Add(1, "A", "B");
            Assert.True(TestObject.TryGetValue(1, out var Values));
            Assert.Equal(2, Values.Count());
            Assert.True(TestObject.TryGetValue("A", out var ValuesInt));
            Assert.Single(ValuesInt);
            TestObject.Remove("B");
            Assert.True(TestObject.TryGetValue(1, out Values));
            Assert.Single(Values);
            Assert.True(TestObject.TryGetValue("A", out ValuesInt));
            Assert.Single(ValuesInt);
            Assert.False(TestObject.TryGetValue("B", out ValuesInt));
            Assert.Empty(ValuesInt);
            TestObject.Remove("A");
            Assert.False(TestObject.TryGetValue(1, out Values));
            Assert.Empty(Values);
            Assert.False(TestObject.TryGetValue("A", out ValuesInt));
            Assert.Empty(ValuesInt);
            Assert.False(TestObject.TryGetValue("B", out ValuesInt));
            Assert.Empty(ValuesInt);
        }

        [Fact]
        public void RemoveLeftToRight()
        {
            var TestObject = new ManyToManyIndex<int, string>();
            TestObject.Add(1, "A", "B");
            Assert.True(TestObject.TryGetValue(1, out var Values));
            Assert.Equal(2, Values.Count());
            Assert.True(TestObject.TryGetValue("A", out var ValuesInt));
            Assert.Single(ValuesInt);
            TestObject.Remove(1);
            Assert.False(TestObject.TryGetValue(1, out Values));
            Assert.Empty(Values);
            Assert.False(TestObject.TryGetValue("A", out ValuesInt));
            Assert.Empty(ValuesInt);
            Assert.False(TestObject.TryGetValue("B", out ValuesInt));
            Assert.Empty(ValuesInt);
        }
    }
}