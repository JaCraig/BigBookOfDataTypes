using BigBook.Comparison;
using BigBook.Tests.BaseClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ConcurrentBagExtensionsTests : TestingDirectoryFixture
    {
        [Fact]
        public void AddAndReturnNull()
        {
            ConcurrentBag<int> TestObject = null;
            const int Item = 7;
            Assert.ThrowsAny<ArgumentNullException>(() => TestObject.AddAndReturn(Item));
        }

        [Fact]
        public void AddAndReturnTest()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            const int Item = 7;
            Assert.Equal(Item, TestObject.AddAndReturn(Item));
        }

        [Fact]
        public void AddIfIEnumerable()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIf(x => x > 1, (IEnumerable<int>)new int[] { 1 }));
            Assert.True(TestObject.AddIf(x => x > 1, (IEnumerable<int>)new int[] { 1, 7 }));
            Assert.True(TestObject.AddIf(x => x > 7, (IEnumerable<int>)new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddIfParams()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIf(x => x > 1, new int[] { 1 }));
            Assert.True(TestObject.AddIf(x => x > 1, new int[] { 1, 7 }));
            Assert.True(TestObject.AddIf(x => x > 7, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddIfParamsNull()
        {
            ConcurrentBag<int> TestObject = null;
            Assert.False(TestObject.AddIf(x => x > 1, new int[] { 1 }));
            Assert.False(TestObject.AddIf(x => x > 1, new int[] { 1, 7 }));
            Assert.False(TestObject.AddIf(x => x > 7, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
        }

        [Fact]
        public void AddIfParamsNull2()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.True(TestObject.AddIf(x => x > 1, Array.Empty<int>()));
            Assert.True(TestObject.AddIf(x => x > 1, null));
        }

        [Fact]
        public void AddIfTest()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIf(x => x > 1, 1));
            Assert.True(TestObject.AddIf(x => x > 1, 7));
            Assert.True(TestObject.AddIf(x => x > 7, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddIfUniqueIEnumerable()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique((IEnumerable<int>)new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique((IEnumerable<int>)new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueIEnumerableWithIEqualityComparer()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique(new GenericEqualityComparer<int>(), (IEnumerable<int>)new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique(new GenericEqualityComparer<int>(), (IEnumerable<int>)new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueIEnumerableWithNullIEqualityComparer()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            GenericEqualityComparer<int> Comparer = null;
            Assert.False(TestObject.AddIfUnique(Comparer, (IEnumerable<int>)new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique(Comparer, (IEnumerable<int>)new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueIEnumerableWithNullPredicate()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Func<int, int, bool> Comparer = null;
            Assert.False(TestObject.AddIfUnique(Comparer, (IEnumerable<int>)new int[] { 1 }));
            Assert.False(TestObject.AddIfUnique(Comparer, (IEnumerable<int>)new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueIEnumerableWithPredicate()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique((x, y) => x == y, (IEnumerable<int>)new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique((x, y) => x == y, (IEnumerable<int>)new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueParamsWithIEqualityComparer()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique(new GenericEqualityComparer<int>(), new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique(new GenericEqualityComparer<int>(), new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueParamsWithNullIEqualityComparer()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            GenericEqualityComparer<int> Comparer = null;
            Assert.False(TestObject.AddIfUnique(Comparer, new int[] { 1 }));
            Assert.True(TestObject.AddIfUnique(Comparer, new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueParamsWithNullPredicate()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Func<int, int, bool> Comparer = null;
            Assert.False(TestObject.AddIfUnique(Comparer, new int[] { 1 }));
            Assert.False(TestObject.AddIfUnique(Comparer, new int[] { 7 }));
        }

        [Fact]
        public void AddIfUniqueTest()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique(1));
            Assert.True(TestObject.AddIfUnique(7));
            Assert.True(TestObject.AddIfUnique(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
            TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.False(TestObject.AddIfUnique((x, y) => x == y, 1));
            Assert.True(TestObject.AddIfUnique((x, y) => x == y, 7));
            Assert.True(TestObject.AddIfUnique((x, y) => x == y, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddNull()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 })
            {
                null
            };
            Assert.Equal(6, TestObject.Count);
        }

        [Fact]
        public void AddNull2()
        {
            ConcurrentBag<int> TestObject = null;
            TestObject = TestObject.Add(1, 2, 3);
            Assert.Equal(3, TestObject.Count);
        }

        [Fact]
        public void AddRange()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            var Results = new ConcurrentBag<int>(TestObject.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(14, Results.Count);
            Assert.Equal(14, TestObject.Count);
        }

        [Fact]
        public void Contains()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.True(TestObject.Contains(4));
            Assert.False(TestObject.Contains(-1));
        }

        [Fact]
        public void ContainsNull()
        {
            ConcurrentBag<int> TestObject = null;
            Assert.False(TestObject.Contains(4));
            Assert.False(TestObject.Contains(-1));
        }

        [Fact]
        public void RemoveNullObject()
        {
            ConcurrentBag<int> TestObject = null;
            Assert.Empty(TestObject.Remove(_ => true));
            TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Equal(6, TestObject.Remove(null).Count);
        }

        [Fact]
        public void RemoveRange()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Empty(TestObject.Remove(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
        }

        [Fact]
        public void RemoveRange2()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Empty(TestObject.Remove(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Single(TestObject.Remove(new int[] { 1, 2, 3, 4, 5 }));
            TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Single(TestObject.Remove(new int[] { 1, 2, 3, 4, 5 }));
        }

        [Fact]
        public void RemoveRangeNullObject()
        {
            ConcurrentBag<int> TestObject = null;
            Assert.Empty(TestObject.Remove(Array.Empty<int>()));
            TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            Assert.Equal(6, TestObject.Remove((IEnumerable<int>)null).Count);
        }

        [Fact]
        public void RemoveTest()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            TestObject = new ConcurrentBag<int>(TestObject.Remove((x) => x % 2 == 0));
            Assert.Equal(3, TestObject.Count);
            foreach (var Item in TestObject)
            {
                Assert.False(Item % 2 == 0);
            }
        }

        [Fact]
        public void RemoveTest2()
        {
            var TestObject = new ConcurrentBag<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            TestObject = new ConcurrentBag<int>(TestObject.Remove((x) => x % 2 == 0));
            Assert.Equal(3, TestObject.Count);
            foreach (var Item in TestObject)
            {
                Assert.False(Item % 2 == 0);
            }
        }
    }
}