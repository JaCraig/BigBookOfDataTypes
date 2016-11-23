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
            var A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            var B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(B.Item1, 12);
            Assert.Equal(B.Item2, "ASDF");
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
            var A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            var B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyLeftToRight(A, B);
            Assert.Equal(B.Item1, 12);
            Assert.Equal(B.Item2, "ASDF");
            A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            TestObject.CopyRightToLeft(B, A);
            Assert.Equal(A.Item1, 13);
            Assert.Equal(A.Item2, "ZXCV");
        }
    }
}