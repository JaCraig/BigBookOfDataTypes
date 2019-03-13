using BenchmarkDotNet.Attributes;
using BigBook.Registration;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class IsTests
    {
        private TestClass[] Data { get; set; }

        [Benchmark]
        public void IsAssignableTest()
        {
            var Result = Data.GetType().IsAssignableFrom(typeof(ITestClass[]));
        }

        [Benchmark]
        public void IsMethodTest()
        {
            var Result = Data.Is<ITestClass[]>();
        }

        [Benchmark(Baseline = true)]
        public void IsTest()
        {
            bool Result = Data is ITestClass[];
        }

        [Benchmark]
        public void IsTypeTest()
        {
            var Result = Data.GetType().Is<ITestClass[]>();
        }

        [GlobalSetup]
        public void Setup()
        {
            Data = new TestClass[100];
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
        }

        private interface ITestClass
        {
        }

        private class TestClass : ITestClass
        {
        }
    }
}