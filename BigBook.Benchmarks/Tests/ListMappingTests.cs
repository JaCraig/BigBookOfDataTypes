using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class ListMappingTests
    {
        private BigBook.Benchmarks.Tests.TestClasses.ListMapping<string, object> NewOne { get; } = new TestClasses.ListMapping<string, object>();

        private BigBook.ListMapping<string, object> Original { get; } = new BigBook.ListMapping<string, object>();

        [Params(1, 10, 100, 1000)]
        public int Count;

        //[Benchmark]
        //public void NewerListAdd()
        //{
        //    Parallel.For(0, 10, x =>
        //      {
        //          NewOne.Add("A", new { A = 1 });
        //          NewOne.Add("B", new { A = 1 });
        //          NewOne.Add("C", new { A = 1 });
        //      });
        //}

        [Benchmark]
        public void NewerListAddAndRead()
        {
            Count.Times(x => NewOne.Add("A", new { A = 1 }));
            NewOne.TryGetValue("A", out var Values);
            NewOne.Clear();
        }

        //[Benchmark(Baseline = true)]
        //public void OriginalListAdd()
        //{
        //    Parallel.For(0, 10, x =>
        //    {
        //        Original.Add("A", new { A = 1 });
        //        Original.Add("B", new { A = 1 });
        //        Original.Add("C", new { A = 1 });
        //    });
        //}

        [Benchmark]
        public void OriginalListAddAndRead()
        {
            Count.Times(x => Original.Add("A", new { A = 1 }));
            Original.TryGetValue("A", out var Values);
            Original.Clear();
        }

        [GlobalSetup]
        public void Setup()
        {
            new ServiceCollection().AddCanisterModules(configure => configure.RegisterBigBookOfDataTypes());
        }
    }
}