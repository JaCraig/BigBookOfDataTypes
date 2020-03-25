using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class StringFormatVsStringBuilder
    {
        private ObjectPool<StringBuilder> Pool { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            Pool = new DefaultObjectPoolProvider().CreateStringBuilderPool();
        }

        [Benchmark]
        public void StringBuilder()
        {
            _ = new StringBuilder().Append("ALTER TABLE [").Append("A").Append("].[").Append("B").Append("] ADD CONSTRAINT [").Append("C").Append("] CHECK (").Append("D").Append(")").ToString();
        }

        [Benchmark]
        public void StringBuilderPool()
        {
            var Builder = Pool.Get();
            _ = Builder.Append("ALTER TABLE [").Append("A").Append("].[").Append("B").Append("] ADD CONSTRAINT [").Append("C").Append("] CHECK (").Append("D").Append(")").ToString();
            Pool.Return(Builder);
        }

        [Benchmark(Baseline = true)]
        public void StringFormat()
        {
            _ = string.Format("ALTER TABLE [{0}].[{1}] ADD CONSTRAINT [{2}] CHECK ({3})", "A", "B", "C", "D");
        }
    }
}