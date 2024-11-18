using BigBook;
using BigBook.Tests.BaseClasses;
using System.Linq;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class MathExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType { get; set; } = typeof(MathExtensions);

        [Fact]
        public void FactorialTest()
        {
            const int Value = 8;
            Assert.Equal(40320, Value.Factorial());
        }

        [Fact]
        public void GCD() => Assert.Equal(3, 9.GreatestCommonDenominator(12));

        [Fact]
        public void MedianTest()
        {
            Assert.Equal(10, new int[] { 9, 11, 10 }.ToList().Median());
            Assert.Equal(10, new int[] { 9, 11, 10 }.ToList().Median((x, y) => (x + y) / 2, x => x));
        }

        [Fact]
        public void ModeTest() => Assert.Equal(20, new int[] { 5, 2, 20, 5, 20, 8, 9, 20, 10 }.ToList().Mode());

        [Fact]
        public void PowTest()
        {
            const double Value = 4;
            Assert.Equal(256, Value.Pow(4));
        }

        [Fact]
        public void Round()
        {
            const double Value = 4.1234;
            Assert.Equal(4.12, Value.Round());
        }

        [Fact]
        public void SqrtTest()
        {
            const double Value = 4;
            Assert.Equal(2, Value.Sqrt());
        }

        [Fact]
        public void StandardDeviationTest()
        {
            Assert.InRange(new double[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
            Assert.InRange(new float[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
            Assert.InRange(new int[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
            Assert.InRange(new decimal[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
            Assert.InRange(new long[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
            var Values = new TestClass[]
            {
                new TestClass{DoubleValue=5,FloatValue=5,IntValue=5,DecimalValue=5,LongValue=5},
                new TestClass{DoubleValue=4,FloatValue=4,IntValue=4,DecimalValue=4,LongValue=4},
                new TestClass{DoubleValue=2,FloatValue=2,IntValue=2,DecimalValue=2,LongValue=2},
                new TestClass{DoubleValue=4,FloatValue=4,IntValue=4,DecimalValue=4,LongValue=4},
                new TestClass{DoubleValue=7,FloatValue=7,IntValue=7,DecimalValue=7,LongValue=7},
                new TestClass{DoubleValue=9,FloatValue=9,IntValue=9,DecimalValue=9,LongValue=9},
                new TestClass{DoubleValue=1,FloatValue=1,IntValue=1,DecimalValue=1,LongValue=1},
                new TestClass{DoubleValue=2,FloatValue=2,IntValue=2,DecimalValue=2,LongValue=2},
                new TestClass{DoubleValue=0,FloatValue=0,IntValue=0,DecimalValue=0,LongValue=0}
            };
            Assert.InRange(Values.StandardDeviation(x => x.DoubleValue), 2.73, 2.74);
            Assert.InRange(Values.StandardDeviation(x => x.FloatValue), 2.73, 2.74);
            Assert.InRange(Values.StandardDeviation(x => x.IntValue), 2.73, 2.74);
            Assert.InRange(Values.StandardDeviation(x => x.DecimalValue), 2.73, 2.74);
            Assert.InRange(Values.StandardDeviation(x => x.LongValue), 2.73, 2.74);
        }

        [Fact]
        public void VarianceTest()
        {
            Assert.InRange(new double[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
            Assert.InRange(new float[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
            Assert.InRange(new int[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
            Assert.InRange(new decimal[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
            Assert.InRange(new long[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
            var Values = new TestClass[]
            {
                new TestClass{DoubleValue=5,FloatValue=5,IntValue=5,DecimalValue=5,LongValue=5},
                new TestClass{DoubleValue=4,FloatValue=4,IntValue=4,DecimalValue=4,LongValue=4},
                new TestClass{DoubleValue=2,FloatValue=2,IntValue=2,DecimalValue=2,LongValue=2},
                new TestClass{DoubleValue=4,FloatValue=4,IntValue=4,DecimalValue=4,LongValue=4},
                new TestClass{DoubleValue=7,FloatValue=7,IntValue=7,DecimalValue=7,LongValue=7},
                new TestClass{DoubleValue=9,FloatValue=9,IntValue=9,DecimalValue=9,LongValue=9},
                new TestClass{DoubleValue=1,FloatValue=1,IntValue=1,DecimalValue=1,LongValue=1},
                new TestClass{DoubleValue=2,FloatValue=2,IntValue=2,DecimalValue=2,LongValue=2},
                new TestClass{DoubleValue=0,FloatValue=0,IntValue=0,DecimalValue=0,LongValue=0}
            };
            Assert.InRange(Values.Variance(x => x.DoubleValue), 7.5, 7.6);
            Assert.InRange(Values.Variance(x => x.FloatValue), 7.5, 7.6);
            Assert.InRange(Values.Variance(x => x.IntValue), 7.5, 7.6);
            Assert.InRange(Values.Variance(x => x.DecimalValue), 7.5, 7.6);
            Assert.InRange(Values.Variance(x => x.LongValue), 7.5, 7.6);
        }

        private class TestClass
        {
            public decimal DecimalValue { get; set; }
            public double DoubleValue { get; set; }

            public float FloatValue { get; set; }

            public int IntValue { get; set; }
            public long LongValue { get; set; }
        }
    }
}