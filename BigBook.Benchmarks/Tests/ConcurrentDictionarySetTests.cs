using BenchmarkDotNet.Attributes;
using BigBook.Registration;
using System.Collections.Concurrent;

namespace BigBook.Benchmarks.Tests
{
    public class ConcurrentDictionarySetTests
    {
        public ConcurrentDictionary<string, object> Data { get; set; }

        [Benchmark(Baseline = true)]
        public void AddOrUpdate() => Data.AddOrUpdate("A", "A", (_, __) => "A");

        [Benchmark]
        public void DoesContainKeyAssign()
        {
            if (Data.ContainsKey("A"))
            {
                Data["A"] = "A";
            }
            else
            {
                Data.AddOrUpdate("A", "A", (_, __) => "A");
            }
        }

        [Benchmark]
        public void DoesNotContainKey()
        {
            if (Data.ContainsKey("B"))
            {
                Data["B"] = "A";
            }
            else
            {
                Data.AddOrUpdate("B", "A", (__, _) => "A");
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            Data = new ConcurrentDictionary<string, object>();
            Data.AddOrUpdate("A", 1, (_, __) => 1);
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }
    }
}