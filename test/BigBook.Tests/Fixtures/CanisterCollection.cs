using FileCurator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace BigBook.Tests.Fixtures
{
    [CollectionDefinition("DirectoryCollection")]
    public class CanisterCollection : ICollectionFixture<CanisterFixture>
    {
    }

    public class CanisterFixture : IDisposable
    {
        public CanisterFixture()
        {
            if (Canister.Builder.Bootstrapper == null)
            {
                new ServiceCollection().AddCanisterModules();
            }
            Canister.Builder.Bootstrapper.Resolve<BigBook.DataMapper.Manager>();

            new DirectoryInfo(@".\Testing").Create();
            new DirectoryInfo(@".\App_Data").Create();
            new DirectoryInfo(@".\Logs").Create();
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\Testing").Delete();
            new DirectoryInfo(@".\App_Data").Delete();
            new DirectoryInfo(@".\Logs").Delete();
        }
    }
}