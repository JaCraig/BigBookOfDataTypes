using BenchmarkDotNet.Attributes;
using BigBook.Reflection;
using BigBook.Registration;
using System;

namespace BigBook.Benchmarks.Tests
{
    public class GetPropertyTests
    {
        [Benchmark(Baseline = true)]
        public void GetPropertyTest()
        {
            var Result = typeof(TestClass).GetProperty("A");
        }

        [Benchmark()]
        public void PropertyLookUpTest()
        {
            var Result = Array.Find(TypeCacheFor<TestClass>.Properties, y => y.Name == "A");
        }

        [GlobalSetup]
        public void Setup()
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }

        private class TestClass
        {
            public string A { get; set; }
            public int B { get; set; }
        }
    }
}