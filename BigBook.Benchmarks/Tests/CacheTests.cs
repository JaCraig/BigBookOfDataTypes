using BenchmarkDotNet.Attributes;
using BigBook.Registration;
using System.Linq;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class CacheTests
    {
        //[Params(1, 10, 100, 1000)]
        public int Count;

        private BigBook.Benchmarks.Tests.TestClasses.Cache NewOne { get; } = new TestClasses.Cache();
        private BigBook.Caching.Default.Cache Original { get; } = new Caching.Default.Cache();

        [Benchmark]
        public void NewerListAddAndRead()
        {
            Count.Times(x => NewOne.Add("A", new { A = 1 }, "A", "B", "C"));
            NewOne.TryGetValue("A", out var Value);
            NewOne.GetByTag("B").ToArray();
            NewOne.Clear();
        }

        [Benchmark]
        public void OriginalListAddAndRead()
        {
            Count.Times(x => Original.Add("A", new { A = 1 }, "A", "B", "C"));
            Original.TryGetValue("A", out var Values);
            Original.GetByTag("B").ToArray();
            Original.Clear();
        }

        [GlobalSetup]
        public void Setup()
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }
    }
}