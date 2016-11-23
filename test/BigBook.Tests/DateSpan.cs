using System;
using Xunit;

namespace BigBook.Tests
{
    public class DateSpanTests
    {
        [Fact]
        public void CompareTest()
        {
            var Span1 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2009, 1, 1));
            var Span2 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2009, 1, 1));
            var Span3 = new BigBook.DateSpan(new DateTime(1999, 1, 2), new DateTime(2009, 1, 1));

            Assert.True(Span1 == Span2);
            Assert.False(Span1 == Span3);
        }

        [Fact]
        public void DifferenceTest()
        {
            var Span1 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            Assert.Equal(4, Span1.Years);
            Assert.Equal(0, Span1.Months);
            Assert.Equal(0, Span1.Days);
            Assert.Equal(0, Span1.Hours);
            Assert.Equal(0, Span1.Minutes);
            Assert.Equal(0, Span1.Seconds);
            Assert.Equal(0, Span1.MilliSeconds);
            var Span2 = new BigBook.DateSpan(new DateTime(1999, 1, 1, 2, 3, 4), new DateTime(2003, 11, 15, 6, 45, 32));
            Assert.Equal(4, Span2.Years);
            Assert.Equal(10, Span2.Months);
            Assert.Equal(14, Span2.Days);
            Assert.Equal(4, Span2.Hours);
            Assert.Equal(42, Span2.Minutes);
            Assert.Equal(28, Span2.Seconds);
            Assert.Equal(0, Span2.MilliSeconds);
        }

        [Fact]
        public void IntersectionTest()
        {
            var Span1 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            var Span2 = new BigBook.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            var Span3 = Span1.Intersection(Span2);
            Assert.Equal(new DateTime(2002, 1, 1), Span3.Start);
            Assert.Equal(new DateTime(2003, 1, 1), Span3.End);
        }

        [Fact]
        public void OverlapTest()
        {
            var Span1 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            var Span2 = new BigBook.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            Assert.True(Span1.Overlap(Span2));
        }

        [Fact]
        public void UnionTest()
        {
            var Span1 = new BigBook.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            var Span2 = new BigBook.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            BigBook.DateSpan Span3 = Span1 + Span2;
            Assert.Equal(new DateTime(1999, 1, 1), Span3.Start);
            Assert.Equal(new DateTime(2009, 1, 1), Span3.End);
        }
    }
}