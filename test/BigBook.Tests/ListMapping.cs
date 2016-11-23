﻿using BigBook;
using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class ListMappingTests
    {
        [Fact]
        public void ContainsTest()
        {
            var TestObject = new ListMapping<string, int>();
            TestObject.Add("A", 0);
            TestObject.Add("A", 1);
            Assert.True(TestObject.Contains("A", 0));
            Assert.False(TestObject.Contains("A", 2));
        }

        [Fact]
        public void RandomTest()
        {
            var TestObject = new ListMapping<string, int>();
            var Rand = new System.Random();
            for (int x = 0; x < 10; ++x)
            {
                var Name = x.ToString();
                for (int y = 0; y < 5; ++y)
                {
                    var Value = Rand.Next();
                    TestObject.Add(Name, Value);
                    Assert.Equal(y + 1, TestObject[Name].Count());
                    Assert.True(TestObject[Name].Contains(Value));
                }
            }
            Assert.Equal(10, TestObject.Count);
        }

        [Fact]
        public void RemoveTest()
        {
            var TestObject = new ListMapping<string, int>();
            TestObject.Add("A", 0);
            TestObject.Add("A", 1);
            TestObject.Remove("A", 0);
            Assert.Equal(1, TestObject["A"].Count());
            Assert.Equal(1, TestObject["A"].FirstOrDefault());
        }
    }
}