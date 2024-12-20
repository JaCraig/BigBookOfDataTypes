﻿using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests
{
    public class BagTests : TestBaseClass<Bag<string>>
    {
        public BagTests()
        {
            TestObject = new Bag<string>();
        }

        [Fact]
        public void RandomTest()
        {
            var BagObject = new Bag<string>();
            var Rand = new System.Random();
            for (var x = 0; x < 10; ++x)
            {
                var Value = x.ToString();
                var Count = Rand.Next(1, 10);
                for (var y = 0; y < Count; ++y)
                {
                    BagObject.Add(Value);
                }

                Assert.Equal(Count, BagObject[Value]);
            }
            Assert.Equal(10, BagObject.Count);
        }
    }
}