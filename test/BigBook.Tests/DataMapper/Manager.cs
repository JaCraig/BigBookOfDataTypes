using BigBook.DataMapper.Interfaces;
using BigBook.Tests.BaseClasses;
using BigBook.Tests.DataMapper.Default;
using System.Collections.Generic;
using Xunit;

namespace BigBook.Tests.DataMapper
{
    public class ManagerTests : TestingDirectoryFixture
    {
        [Fact]
        public void CreationTest()
        {
            BigBook.DataMapper.Manager TestObject = null;
            TestObject = new BigBook.DataMapper.Manager(new List<IDataMapper>(), new List<IMapperModule>());
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void TypeMappingTest()
        {
            var TestObject = new BigBook.DataMapper.Manager(new List<IDataMapper>(), new List<IMapperModule>());
            Assert.NotNull(TestObject.Map<MappingA, MappingB>());
            Assert.IsType<BigBook.DataMapper.Default.TypeMapping<MappingA, MappingB>>(TestObject.Map<MappingA, MappingB>());
        }
    }
}