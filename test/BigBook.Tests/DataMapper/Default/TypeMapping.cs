using BigBook.DataMapper.Default;
using Xunit;

namespace BigBook.Tests.DataMapper.Default
{
    public class TypeMappingTests
    {
        [Fact]
        public void AutoMapping()
        {
            BigBook.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
            TestObject.AutoMap();
            var A = new MappingA
            {
                Item1 = 12,
                Item2 = "ASDF"
            };
            var B = new MappingB
            {
                Item1 = 13,
                Item2 = "ZXCV"
            };
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(12, B.Item1);
            Assert.Equal("ASDF", B.Item2);
        }

        [Fact]
        public void CreationTest()
        {
            BigBook.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void Mapping()
        {
            BigBook.DataMapper.Default.TypeMapping<MappingA, MappingB> TestObject = null;
            TestObject = new TypeMapping<MappingA, MappingB>();
            Assert.NotNull(TestObject);
            TestObject.AddMapping(x => x.Item1, x => x.Item1);
            TestObject.AddMapping(x => x.Item2, x => x.Item2);
            var A = new MappingA
            {
                Item1 = 12,
                Item2 = "ASDF"
            };
            var B = new MappingB
            {
                Item1 = 13,
                Item2 = "ZXCV"
            };
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(12, B.Item1);
            Assert.Equal("ASDF", B.Item2);
            A = new MappingA
            {
                Item1 = 12,
                Item2 = "ASDF"
            };
            B = new MappingB
            {
                Item1 = 13,
                Item2 = "ZXCV"
            };
            TestObject.CopyRightToLeft(B, A);
            Assert.Equal(13, A.Item1);
            Assert.Equal("ZXCV", A.Item2);
        }
    }
}