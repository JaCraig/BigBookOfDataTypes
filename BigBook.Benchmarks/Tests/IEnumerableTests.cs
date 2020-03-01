using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.Benchmarks.Tests
{
    public static class NewExtensions
    {
        public static IEnumerable<TResult> ForNew<TObject, TResult>(this IEnumerable<TObject> list, int start, int end, Func<TObject, int, TResult> function)
        {
            if (list is null || function is null || !list.Any())
            {
                return Array.Empty<TResult>();
            }

            int Count = 0;
            var ReturnList = new TResult[end + 1 - start];

            foreach (var Item in list.ElementsBetween(start, end + 1))
            {
                ReturnList[Count] = function(Item, Count);
                ++Count;
            }
            return ReturnList;
        }
    }

    [MemoryDiagnoser]
    public class IEnumerableTests
    {
        [Params(10, 100, 1000, 10000)]
        public int Count;

        private static List<int> List1 { get; set; }

        private static List<int> List2 { get; set; }

        private static List<int> List3 { get; set; }

        [Benchmark]
        public void New()
        {
            _ = ((IEnumerable<int>)List1).ForNew(List1.Count / 4, List1.Count / 2, (x, y) => x);
        }

        [Benchmark(Baseline = true)]
        public void Old()
        {
            _ = ((IEnumerable<int>)List1).For(List1.Count / 4, List1.Count / 2, (x, y) => x);
        }

        [GlobalSetup]
        public void Setup()
        {
            List1 = new List<int>();
            List2 = new List<int>();
            List3 = new List<int>();
            for (int x = 0; x < Count; ++x)
            {
                List1.Add(x);
                List2.Add(x);
                List3.Add(x);
            }
        }
    }
}