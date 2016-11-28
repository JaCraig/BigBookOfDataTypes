﻿using System;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void AddWeeks()
        {
            Assert.Equal(new DateTime(2009, 1, 15, 2, 3, 4), new DateTime(2009, 1, 1, 2, 3, 4).AddWeeks(2));
        }

        [Fact]
        public void Age()
        {
            Assert.Equal(41, new DateTime(1940, 1, 1).Age(new DateTime(1981, 1, 1)));
        }

        [Fact]
        public void BeginningOf()
        {
            Assert.Equal(new DateTime(1999, 1, 1), new DateTime(1999, 1, 2).BeginningOf(TimeFrame.Month));
            Assert.Equal(new DateTime(2009, 1, 1), new DateTime(2009, 1, 15, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 4, 1), new DateTime(2009, 4, 1, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 1, 1), new DateTime(2009, 3, 29, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 7, 1), new DateTime(2009, 7, 1, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 4, 1), new DateTime(2009, 6, 29, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 10, 1), new DateTime(2009, 10, 1, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 7, 1), new DateTime(2009, 9, 29, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2010, 1, 1), new DateTime(2010, 1, 1).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 10, 1), new DateTime(2009, 12, 31, 2, 3, 4).BeginningOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 1, 1, 0, 0, 0), new DateTime(2009, 1, 15, 2, 3, 4).BeginningOf(TimeFrame.Year));
            Assert.Equal(new DateTime(1998, 12, 27), new DateTime(1999, 1, 2).BeginningOf(TimeFrame.Week));
        }

        [Fact]
        public void ConvertToTimeZone()
        {
            Assert.Equal(new DateTime(2009, 1, 14, 18, 3, 4), new DateTime(2009, 1, 15, 2, 3, 4, DateTimeKind.Utc).To(TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")));
        }

        [Fact]
        public void DaysIn()
        {
            Assert.Equal(31, new DateTime(1999, 1, 2).DaysIn(TimeFrame.Month));
            Assert.Equal(365, new DateTime(1999, 1, 2).DaysIn(TimeFrame.Year));
            Assert.Equal(7, new DateTime(1999, 1, 2).DaysIn(TimeFrame.Week));
            Assert.Equal(89, new DateTime(1999, 1, 2).DaysIn(TimeFrame.Quarter));
            Assert.Equal(1, new DateTime(1999, 1, 2).DaysIn(TimeFrame.Day));
        }

        [Fact]
        public void DaysLeftIn()
        {
            Assert.Equal(29, new DateTime(1999, 1, 2).DaysLeftIn(TimeFrame.Month));
            Assert.Equal(363, new DateTime(1999, 1, 2).DaysLeftIn(TimeFrame.Year));
            Assert.Equal(0, new DateTime(1999, 1, 2).DaysLeftIn(TimeFrame.Week));
            Assert.Equal(88, new DateTime(1999, 1, 2).DaysLeftIn(TimeFrame.Quarter));
            Assert.Equal(1, new DateTime(1999, 1, 2).DaysLeftIn(TimeFrame.Day));
        }

        [Fact]
        public void EndOf()
        {
            Assert.Equal(new DateTime(1999, 1, 2, 23, 59, 59), new DateTime(1999, 1, 2, 12, 1, 1).EndOf(TimeFrame.Day));
            Assert.Equal(new DateTime(1999, 1, 2), new DateTime(1999, 1, 2).EndOf(TimeFrame.Week));
            Assert.Equal(new DateTime(2009, 12, 31, 0, 0, 0), new DateTime(2009, 1, 15, 2, 3, 4).EndOf(TimeFrame.Year));
            Assert.Equal(new DateTime(1999, 1, 31), new DateTime(1999, 1, 2).EndOf(TimeFrame.Month));
            Assert.Equal(new DateTime(2009, 3, 31), new DateTime(2009, 1, 15, 2, 3, 4).EndOf(TimeFrame.Quarter));
            Assert.Equal(new DateTime(2009, 6, 30), new DateTime(2009, 4, 1, 2, 3, 4).EndOf(TimeFrame.Quarter));
        }

        [Fact]
        public void FromUnix()
        {
            Assert.Equal(new DateTime(2009, 2, 13, 23, 31, 30), 1234567890.To());
        }

        [Fact]
        public void IsInFutureTest()
        {
            Assert.True(new DateTime(2100, 1, 1).Is(DateCompare.InFuture));
        }

        [Fact]
        public void IsInPastTest()
        {
            Assert.True(new DateTime(1900, 1, 1).Is(DateCompare.InPast));
        }

        [Fact]
        public void IsToday()
        {
            Assert.True(DateTime.Now.Is(DateCompare.Today));
        }

        [Fact]
        public void IsWeekDayTest()
        {
            Assert.False(new DateTime(1999, 1, 2).Is(DateCompare.WeekDay));
        }

        [Fact]
        public void IsWeekEndTest()
        {
            Assert.True(new DateTime(1999, 1, 2).Is(DateCompare.WeekEnd));
        }

        [Fact]
        public void RelativeTime()
        {
            Assert.Equal("34 years, 11 months from now", new DateTime(2011, 12, 1).ToString(new DateTime(1977, 1, 1)));
            Assert.Equal("34 years, 11 months ago", new DateTime(1977, 1, 1).ToString(new DateTime(2011, 12, 1)));
        }

        [Fact]
        public void SetTime()
        {
            Assert.Equal(new DateTime(2009, 1, 1, 14, 2, 12), new DateTime(2009, 1, 1, 2, 3, 4).SetTime(14, 2, 12));
        }

        [Fact]
        public void ToUnix()
        {
            Assert.Equal(915148800, new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc).To());
        }

        [Fact]
        public void UTCOffset()
        {
            Assert.Equal(DateTime.Now.Hour - DateTime.UtcNow.Hour, new DateTime(1999, 1, 2, 23, 1, 1, DateTimeKind.Local).UTCOffset());
        }
    }
}