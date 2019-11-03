using BenchmarkDotNet.Attributes;
using BigBook.Registration;
using System.Collections.Concurrent;
using System.Dynamic;

namespace BigBook.Benchmarks.Tests
{
    [RPlotExporter, RankColumn, MemoryDiagnoser]
    public class DynamoTests
    {
        [Params(1000, 10000)]
        public int Count;

        public object[] Data { get; set; }

        [Benchmark]
        public object[] AnnonymousTest()
        {
            Data = new object[Count];
            for (var x = 0; x < Count; ++x)
            {
                Data[x] = new { A = "Test data goes here" };
            }
            return Data;
        }

        [Benchmark(Baseline = true)]
        public object[] ClassTest()
        {
            Data = new TestClass[Count];
            for (var x = 0; x < Count; ++x)
            {
                Data[x] = new TestClass { A = "Test data goes here" };
            }
            return Data;
        }

        [Benchmark]
        public object[] ConcurrentDictionaryConversionTest()
        {
            Data = new ConcurrentDictionary<string, object>[Count];
            for (var x = 0; x < Count; ++x)
            {
                var Temp = new ConcurrentDictionary<string, object>();
                Temp.AddOrUpdate("A", "Test data goes here", (_, __) => "Test data goes here");
                Data[x] = Temp;
            }
            return Data;
        }

        [Benchmark]
        public object[] DynamoAnonymousConversionTest()
        {
            Data = new Dynamo[Count];
            for (var x = 0; x < Count; ++x)
            {
                Data[x] = new Dynamo(new { A = "Test data goes here" });
            }
            return Data;
        }

        [Benchmark]
        public object[] DynamoConversionTest()
        {
            Data = new Dynamo[Count];
            for (var x = 0; x < Count; ++x)
            {
                Data[x] = new Dynamo(new TestClass { A = "Test data goes here" });
            }
            return Data;
        }

        [Benchmark]
        public object[] DynamoExpandoConversionTest()
        {
            Data = new Dynamo[Count];
            for (var x = 0; x < Count; ++x)
            {
                dynamic Temp = new ExpandoObject();
                Temp.A = "Test data goes here";
                Data[x] = new Dynamo(Temp);
            }
            return Data;
        }

        [Benchmark]
        public object[] DynamoTest()
        {
            Data = new Dynamo[Count];
            for (var x = 0; x < Count; ++x)
            {
                dynamic Temp = new Dynamo();
                Temp.A = "Test data goes here";
                Data[x] = Temp;
            }
            return Data;
        }

        [Benchmark]
        public object[] ExpandoTest()
        {
            Data = new ExpandoObject[Count];
            for (var x = 0; x < Count; ++x)
            {
                dynamic Temp = new ExpandoObject();
                Temp.A = "Test data goes here";
                Data[x] = Temp;
            }
            return Data;
        }

        [GlobalSetup]
        public void Setup() => Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();

        private class TestClass
        {
            public string A { get; set; }
        }
    }
}