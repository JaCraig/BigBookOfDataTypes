using BigBook.ExtensionMethods;
using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class CacheExtensionTests : TestingDirectoryFixture
    {
        [Fact]
        public void Cache()
        {
            const int A = 1;
            A.Cache("A");
            Assert.Equal(A, "A".GetFromCache<int>());
        }
    }
}