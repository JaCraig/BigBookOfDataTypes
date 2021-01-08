using BenchmarkDotNet.Attributes;
using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class CacheTests
    {
        private IAppCache LazyCacheService { get; set; }
        private ManyToManyIndex<int, string> LazyIndex { get; set; } = new ManyToManyIndex<int, string>();
        private BigBook.Benchmarks.Tests.TestClasses.Cache NewOne { get; } = new TestClasses.Cache();

        private BigBook.Caching.Default.Cache Original { get; } = new Caching.Default.Cache();

        [Params(1, 10, 100, 1000)]
        public int Count;

        //[Benchmark]
        //public void LazyCacheAddAndRead()
        //{
        //    Count.Times(x =>
        //    {
        //        LazyCacheService.Add("A", new { A = 1 });
        //        //LazyIndex.Add("A".GetHashCode(), new string[] { "A", "B", "C" });
        //    });
        //    var Values = LazyCacheService.Get<object>("A");
        //    //LazyIndex.TryGetValue("A".GetHashCode(), out var Tags);
        //    //Tags.Select(x => LazyCacheService.Get<object>(x)).ToArray();
        //    //LazyIndex.Clear();
        //    LazyCacheService.Remove("A");
        //}

        [Benchmark]
        public void NewerListAddAndRead()
        {
            Count.Times(x => NewOne.Add("A", new { A = 1 }, "A", "B", "C"));
            NewOne.TryGetValue("A", out var Value);
            NewOne.GetByTag("B").ToArray();
            NewOne.Clear();
        }

        [Benchmark(Baseline = true)]
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
            LazyCacheService = new CachingService();
            new ServiceCollection().AddCanisterModules(configure => configure.RegisterBigBookOfDataTypes());
        }
    }
}