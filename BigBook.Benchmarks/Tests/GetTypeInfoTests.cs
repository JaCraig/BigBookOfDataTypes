using BenchmarkDotNet.Attributes;
using BigBook.Reflection;

namespace BigBook.Benchmarks.Tests
{
    public class GetTypeInfoTests
    {
        [Benchmark]
        public void CachedTypeOf()
        {
            var TypeInfo = TypeCacheFor<GetTypeInfoTests>.Type;
        }

        [Benchmark(Baseline = true)]
        public void TypeOf()
        {
            var TypeInfo = typeof(GetTypeInfoTests);
        }
    }
}