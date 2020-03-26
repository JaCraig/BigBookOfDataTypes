using Microsoft.Extensions.ObjectPool;
using System.Text;
using Xunit;

namespace BigBook.Tests.BaseClasses
{
    [Collection("DirectoryCollection")]
    public class TestingDirectoryFixture
    {
        protected static Aspectus.Aspectus AOPManager => Canister.Builder.Bootstrapper.Resolve<Aspectus.Aspectus>();
        protected static ObjectPool<StringBuilder> BuilderPool => Canister.Builder.Bootstrapper.Resolve<ObjectPool<StringBuilder>>();

        protected static BigBook.DataMapper.Manager DataMapper => Canister.Builder.Bootstrapper.Resolve<BigBook.DataMapper.Manager>();
    }
}