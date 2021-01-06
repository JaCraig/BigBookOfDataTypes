using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace BigBook.Benchmarks.Tests
{
    public class ConcurrentDictionaryTests
    {
        public ConcurrentDictionary<string, object> Data { get; set; }

        [Benchmark(Baseline = true)]
        public void ContainsKey()
        {
            if (Data.ContainsKey("A"))
            {
                if (Data.TryGetValue("A", out var Value))
                {
                }
            }
        }

        [Benchmark]
        public void ContainsKeyIndex()
        {
            if (Data.ContainsKey("A"))
            {
                var Value = Data["A"];
            }
        }

        [Benchmark]
        public void DoesNotContainsKey()
        {
            if (Data.ContainsKey("B"))
            {
                if (Data.TryGetValue("B", out var Value))
                {
                }
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            Data = new ConcurrentDictionary<string, object>();
            Data.AddOrUpdate("A", 1, (_, __) => 1);
            new ServiceCollection().AddCanisterModules(configure => configure.RegisterBigBookOfDataTypes());
        }

        [Benchmark]
        public void TryGetValue()
        {
            if (Data.TryGetValue("A", out var Value))
            {
            }
        }
    }
}