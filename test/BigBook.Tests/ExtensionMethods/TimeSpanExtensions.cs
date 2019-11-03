using System;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class TimeSpanExtensionsTests
    {
        [Fact]
        public void Average() => Assert.Equal(new TimeSpan(20), new TimeSpan[] { new TimeSpan(10), new TimeSpan(30) }.Average());

        [Fact]
        public void DaysRemainder() => Assert.Equal(0, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).DaysRemainder());

        [Fact]
        public void Months() => Assert.Equal(11, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).Months());

        [Fact]
        public void ToStringFull() => Assert.Equal("34 years, 11 months", (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).ToStringFull());

        [Fact]
        public void Years() => Assert.Equal(34, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).Years());
    }
}