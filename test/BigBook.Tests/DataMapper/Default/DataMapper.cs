using Xunit;

namespace BigBook.Tests.DataMapper.Default
{
    public class DataMapperTests
    {
        [Fact]
        public void Creation()
        {
            var Manager = new BigBook.DataMapper.Default.DataMapper();
            Manager.Map<MappingA, MappingB>()
                .AddMapping(x => x.Item1, x => x.Item1)
                .AddMapping(x => x.Item2, x => x.Item2);
            var A = new MappingA();
            A.Item1 = 12;
            A.Item2 = "ASDF";
            var B = new MappingB();
            B.Item1 = 13;
            B.Item2 = "ZXCV";
            Manager.Map<MappingA, MappingB>().CopyLeftToRight(A, B);
            Assert.Equal(B.Item1, 12);
            Assert.Equal(B.Item2, "ASDF");
        }
    }
}