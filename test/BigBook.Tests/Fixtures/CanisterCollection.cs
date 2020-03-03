using BigBook.Registration;
using BigBook.Tests.BaseClasses;
using FileCurator;
using FileCurator.Registration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestFountain.Registration;
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
                Canister.Builder.CreateContainer(new ServiceCollection())
                    .AddAssembly(typeof(TestingDirectoryFixture).Assembly)
                    .RegisterBigBookOfDataTypes()
                    .RegisterFileCurator()
                    .RegisterTestFountain()
                    .Build();
            }

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