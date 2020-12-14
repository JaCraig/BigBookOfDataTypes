using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class ValueTypeCreation
    {
        private static readonly Dictionary<int, object> HashLookUp = new Dictionary<int, object>
        {
            [typeof(int).GetHashCode()] = 0
        };

        private static readonly Dictionary<Type, object> TypeLookUp = new Dictionary<Type, object>
        {
            [typeof(int)] = 0
        };

        [Benchmark(Baseline = true)]
        public void ActivatorUsage()
        {
            _ = Activator.CreateInstance(typeof(int));
        }

        [Benchmark]
        public void DictionaryHashLookUp()
        {
            _ = HashLookUp[typeof(int).GetHashCode()];
        }

        [Benchmark]
        public void DictionaryTypeLookUp()
        {
            _ = TypeLookUp[typeof(int)];
        }
    }
}