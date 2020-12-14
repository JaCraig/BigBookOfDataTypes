using BigBook.Caching.Default;
using BigBook.Caching.Interfaces;
using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.Caching
{
    public class ManagerTests : TestingDirectoryFixture
    {
        [Fact]
        public void Create()
        {
            using var Test = new BigBook.Caching.Manager(new ICache[] { new Cache() });
            var Temp = Test.Cache();
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);

            var Temp2 = Test.Cache();
            Assert.Equal(Temp, Temp2);
        }
    }
}