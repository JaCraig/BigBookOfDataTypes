using Xunit;

namespace BigBook.IO.Tests
{
    public class BitReaderTests
    {
        [Fact]
        public void Creation()
        {
            using (var TestObject = new BitReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.NotNull(TestObject);
            }
        }

        [Fact]
        public void ReadBit()
        {
            using (var TestObject = new BitReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.NotNull(TestObject);
                var Value = TestObject.ReadBit();
                Assert.True(Value.HasValue);
                Assert.False(Value.Value);
                for (var x = 0; x < 6; ++x)
                {
                    Value = TestObject.ReadBit();
                    Assert.True(Value.HasValue);
                    Assert.False(Value.Value);
                }
                Value = TestObject.ReadBit();
                Assert.True(Value.HasValue);
                Assert.True(Value.Value);
            }
        }

        [Fact]
        public void ReadBitBigEndian()
        {
            using (var TestObject = new BitReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.NotNull(TestObject);
                var Value = TestObject.ReadBit(true);
                Assert.True(Value.HasValue);
                Assert.True(Value.Value);
                for (var x = 0; x < 7; ++x)
                {
                    Value = TestObject.ReadBit(true);
                    Assert.True(Value.HasValue);
                    Assert.False(Value.Value);
                }
            }
        }

        [Fact]
        public void Skip()
        {
            using (var TestObject = new BitReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.NotNull(TestObject);
                var Value = TestObject.ReadBit();
                Assert.True(Value.HasValue);
                Assert.False(Value.Value);
                TestObject.Skip(6);
                Value = TestObject.ReadBit();
                Assert.True(Value.HasValue);
                Assert.True(Value.Value);
            }
        }

        [Fact]
        public void SkipBigEndian()
        {
            using (var TestObject = new BitReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.NotNull(TestObject);
                var Value = TestObject.ReadBit(true);
                Assert.True(Value.HasValue);
                Assert.True(Value.Value);
                TestObject.Skip(6);
                Value = TestObject.ReadBit(true);
                Assert.True(Value.HasValue);
                Assert.False(Value.Value);
            }
        }
    }
}