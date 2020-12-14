using BenchmarkDotNet.Attributes;

namespace BigBook.Benchmarks.Tests
{
    public class NullEqualityTests
    {
        private TestClass? Data { get; }

        [Benchmark]
        public bool Equals() => Data == null;

        [Benchmark]
        public bool EqualsFunc() => Equals(Data, null);

        [Benchmark]
        public bool EqualsObject() => ((object)Data) == null;

        [Benchmark(Baseline = true)]
        public bool Is() => Data is null;

        [Benchmark]
        public bool ReferenceEquals() => ReferenceEquals(Data, null);

        private class TestClass
        {
            public int ID { get; set; }

            public static bool operator !=(TestClass? left, TestClass? right)
            {
                return !(left == right);
            }

            public static bool operator ==(TestClass? left, TestClass? right)
            {
                if (left is null && right is null)
                    return true;
                if (left is null || right is null)
                    return false;
                return left.ID.GetHashCode() == right.ID.GetHashCode();
            }

            public override bool Equals(object? obj)
            {
                var TempObj = obj as TestClass;
                return this == TempObj;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

            public override string? ToString() => base.ToString();
        }
    }
}