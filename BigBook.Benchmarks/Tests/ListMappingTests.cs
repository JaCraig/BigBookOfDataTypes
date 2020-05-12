using BenchmarkDotNet.Attributes;
using BigBook.Registration;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class ListMappingTests
    {
        private BigBook.ListMapping<string, object> Original { get; } = new BigBook.ListMapping<string, object>();

        [Benchmark]
        public void NewerListMapping()
        {
            _ = Data.GetType().Is<ITestClass[]>();
        }

        [Benchmark(Baseline = true)]
        public void OriginalListMapping()
        {
            _ = Data is ITestClass[];
        }

        [GlobalSetup]
        public void Setup()
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }
    }
}