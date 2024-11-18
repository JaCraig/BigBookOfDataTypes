using BigBook.Tests.BaseClasses;
using Mecha.xUnit;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BigBook.Tests
{
    public class BloomFilterTests : TestBaseClass<BloomFilter<string>>
    {
        public BloomFilterTests()
        {
            TestObject = new BloomFilter<string>(1000);
        }

        [Property(GenerationCount = 100)]
        public void Add(int size, [Required] string value)
        {
            if (size == int.MinValue)
                size = int.MaxValue;
            size = Math.Abs(size);
            if (size < 10)
                size = 10;
            else if (size > 1000000)
                size = 1000000;
            var TestObject = new BloomFilter<string>(size);
            TestObject.Add(value);
        }

        [Property(GenerationCount = 100)]
        public void Contains(int size, [Required] string value)
        {
            if (size == int.MinValue)
                size = int.MaxValue;
            size = Math.Abs(size);
            if (size < 10)
                size = 10;
            else if (size > 1000000)
                size = 1000000;
            var TestObject = new BloomFilter<string>(size);
            TestObject.Add(value);
            Assert.True(TestObject.Contains(value));
        }

        [Property(GenerationCount = 100)]
        public void Creation(int size)
        {
            if (size == int.MinValue)
                size = int.MaxValue;
            size = Math.Abs(size);
            if (size < 10)
                size = 10;
            else if (size > 1000000)
                size = 1000000;
            var TestObject = new BloomFilter<string>(size);
        }
    }
}