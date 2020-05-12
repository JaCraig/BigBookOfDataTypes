using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BigBook.Tests.Caching.Default
{
    public class CacheTests
    {
        [Fact]
        public void Add()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1 }
            };
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.Equal(1, Temp["A"]);
            Assert.True(Temp.ContainsKey("A"));
            Assert.True(Temp.Contains(new KeyValuePair<string, object>("A", 1)));
        }

        [Fact]
        public void Clear()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1 }
            };
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Temp.Clear();
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
        }

        [Fact]
        public void Create()
        {
            var Temp = new BigBook.Caching.Default.Cache();
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
        }

        [Fact]
        public void Remove()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1 }
            };
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.Equal(1, Temp["A"]);
            Assert.True(Temp.Remove("A"));
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
            Assert.Null(Temp["A"]);
        }

        [Fact]
        public void TagAdd()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1, new string[] { "Tag1", "Tag2" } }
            };
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.Equal(1, Temp["A"]);
            Assert.True(Temp.ContainsKey("A"));
            Assert.True(Temp.Contains(new KeyValuePair<string, object>("A", 1)));
            Assert.Equal(2, Temp.Tags.Count());
            Assert.Contains("Tag1".GetHashCode(System.StringComparison.Ordinal), Temp.Tags);
            Assert.Contains("Tag2".GetHashCode(System.StringComparison.Ordinal), Temp.Tags);
        }

        [Fact]
        public void TagGet()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1, new string[] { "Tag1", "Tag2" } },
                { "B", 2, new string[] { "Tag2" } },
                { "C", 3 }
            };
            Assert.Equal(2, Temp.GetByTag("Tag2").Count());
            Assert.Single(Temp.GetByTag("Tag1"));
            Assert.Empty(Temp.GetByTag("Tag3"));
            Assert.Equal(1, Temp.GetByTag("Tag1").First());
        }

        [Fact]
        public void TagRemove()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1, new string[] { "Tag1", "Tag2" } },
                { "B", 2, new string[] { "Tag2" } },
                { "C", 3 }
            };
            Temp.RemoveByTag("Tag1");
            Assert.Single(Temp.GetByTag("Tag2"));
            Assert.Empty(Temp.GetByTag("Tag1"));
            Assert.Equal(2, Temp.GetByTag("Tag2").First());
            Assert.Single(Temp.Tags);
            Assert.Equal(2, Temp.Keys.Count);
            Temp.RemoveByTag("Tag1");
        }

        [Fact]
        public void TryGetValue()
        {
            var Temp = new BigBook.Caching.Default.Cache
            {
                { "A", 1 }
            };
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.True(Temp.TryGetValue("A", out var Value));
            Assert.Equal(1, Value);
        }
    }
}