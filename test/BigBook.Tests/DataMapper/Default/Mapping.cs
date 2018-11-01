using BigBook.DataMapper.Default;
using Xunit;

namespace BigBook.Tests.DataMapper.Default
{
    public class MappingA
    {
        public static string StaticItem1 { get; set; }
        public int Item1 { get; set; }

        public string Item2 { get; set; }
    }

    public class MappingB
    {
        public int Item1 { get; set; }

        public string Item2 { get; set; }
    }

    public class MappingTests
    {
        [Fact]
        public void CreationTest()
        {
            Mapping<MappingA, MappingB> TempObject = new Mapping<MappingA, MappingB>(x => x.Item1, x => x.Item1);
            Assert.NotNull(TempObject);
        }

        [Fact]
        public void LeftToRight()
        {
            var TempObject = new Mapping<MappingA, MappingB>(x => x.Item1, x => x.Item1);
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
            TempObject.CopyLeftToRight(A, B);
            Assert.Equal(12, B.Item1);
            Assert.NotEqual("ASDF", B.Item2);
        }

        [Fact]
        public void NullLeftToRight()
        {
            var TempObject = new Mapping<MappingA, MappingB>(null, x => x.Item1);
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
            TempObject.CopyLeftToRight(A, B);
            Assert.Equal(13, B.Item1);
            Assert.Equal("ZXCV", B.Item2);
        }

        [Fact]
        public void NullRightToLeft()
        {
            var TempObject = new Mapping<MappingA, MappingB>(x => x.Item1, null);
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
            TempObject.CopyRightToLeft(B, A);
            Assert.Equal(12, A.Item1);
            Assert.Equal("ASDF", A.Item2);
        }

        [Fact]
        public void RightToLeft()
        {
            var TempObject = new Mapping<MappingA, MappingB>(x => x.Item1, x => x.Item1);
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
            TempObject.CopyRightToLeft(B, A);
            Assert.Equal(13, A.Item1);
            Assert.NotEqual("ZXCV", A.Item2);
        }
    }
}