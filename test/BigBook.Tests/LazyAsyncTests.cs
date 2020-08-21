using BigBook.Tests.BaseClasses;
using System.Threading.Tasks;
using Xunit;

namespace BigBook.Tests
{
    public class LazyAsyncTests : TestingDirectoryFixture
    {
        [Fact]
        public async Task AwaitTest()
        {
            var TestObject = new LazyAsync<int>(() => 5);
            Assert.Equal(5, await TestObject);

            TestObject = new LazyAsync<int>(GetValue);
            Assert.Equal(50, await TestObject);
        }

        private Task<int> GetValue()
        {
            return Task.FromResult(50);
        }
    }
}