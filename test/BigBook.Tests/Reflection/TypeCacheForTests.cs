using BigBook.Reflection;
using Xunit;

namespace BigBook.Tests.Reflection
{
    public class TypeCacheForTests
    {
        [Fact]
        public void BasicTest()
        {
            Assert.Single(TypeCacheFor<SetTests>.Constructors);
            Assert.Empty(TypeCacheFor<SetTests>.Fields);
            Assert.Empty(TypeCacheFor<SetTests>.Interfaces);
            Assert.NotEmpty(TypeCacheFor<SetTests>.Methods);
            Assert.Empty(TypeCacheFor<SetTests>.Properties);
            Assert.NotNull(TypeCacheFor<SetTests>.Type);
        }
    }
}