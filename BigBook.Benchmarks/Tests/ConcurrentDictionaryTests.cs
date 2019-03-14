using BenchmarkDotNet.Attributes;
using BigBook.Registration;
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
                if (Data.TryGetValue("A", out object Value))
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
                if (Data.TryGetValue("B", out object Value))
                {
                }
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            Data = new ConcurrentDictionary<string, object>();
            Data.AddOrUpdate("A", 1, (x, y) => 1);
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }

        [Benchmark]
        public void TryGetValue()
        {
            if (Data.TryGetValue("A", out object Value))
            {
            }
        }
    }
}