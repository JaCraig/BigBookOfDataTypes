using BigBook.DataMapper.Default;
using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.DataMapper.Default
{
    public class DataMapperTests : TestBaseClass<DefaultDataMapper>
    {
        public DataMapperTests()
        {
            TestObject = new DefaultDataMapper();
        }

        [Fact]
        public void Creation()
        {
            var Manager = new BigBook.DataMapper.Default.DefaultDataMapper();
            Manager.Map<MappingA, MappingB>()
                .AddMapping(x => x.Item1, x => x.Item1)
                .AddMapping(x => x.Item2, x => x.Item2);
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
            Manager.Map<MappingA, MappingB>().CopyLeftToRight(A, B);
            Assert.Equal(12, B.Item1);
            Assert.Equal("ASDF", B.Item2);
        }
    }
}