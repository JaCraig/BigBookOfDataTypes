/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.ComponentModel;
using System.Globalization;

namespace BigBook
{
    /// <summary>
    /// Date comparison type
    /// </summary>
    [Flags]
    public enum DateCompare
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// In the future
        /// </summary>
        InFuture = 1,

        /// <summary>
        /// In the past
        /// </summary>
        InPast = 2,

        /// <summary>
        /// Today
        /// </summary>
        Today = 4,

        /// <summary>
        /// Weekday
        /// </summary>
        WeekDay = 8,

        /// <summary>
        /// Weekend
        /// </summary>
        WeekEnd = 16
    }

    /// <summary>
    /// Time frame
    /// </summary>
    public enum TimeFrame
    {
        /// <summary>
        /// Day
        /// </summary>
        Day,

        /// <summary>
        /// Week
        /// </summary>
        Week,

        /// <summary>
        /// Month
        /// </summary>
        Month,

        /// <summary>
        /// Quarter
        /// </summary>
        Quarter,

        /// <summary>
        /// Year
        /// </summary>
        Year
    }

    /// <summary>
    /// DateTime extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Adds the number of weeks to the date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="numberOfWeeks">Number of weeks to add</param>
        /// <returns>The date after the number of weeks are added</returns>
        public static DateTime AddWeeks(this DateTime date, int numberOfWeeks) => date.AddDays(numberOfWeeks * 7);

        /// <summary>
        /// Calculates age based on date supplied
        /// </summary>
        /// <param name="date">Birth date</param>
        /// <param name="calculateFrom">Date to calculate from</param>
        /// <returns>The total age in years</returns>
        public static int Age(this DateTime date, DateTime calculateFrom = default)
        {
            if (calculateFrom == default)
            {
                calculateFrom = DateTime.Now;
            }

            return (calculateFrom - date).Years();
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime date, TimeFrame timeFrame, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return timeFrame switch
            {
                TimeFrame.Day => date.Date,
                TimeFrame.Week => date.AddDays(culture.DateTimeFormat.FirstDayOfWeek - date.DayOfWeek).Date,
                TimeFrame.Month => new DateTime(date.Year, date.Month, 1),
                TimeFrame.Quarter => date.BeginningOf(TimeFrame.Quarter, date.BeginningOf(TimeFrame.Year, culture), culture),
                _ => new DateTime(date.Year, 1, 1),
            };
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo? culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
            {
                return date.BeginningOf(timeFrame, culture);
            }

            culture ??= CultureInfo.CurrentCulture;
            if (date.Between(startOfQuarter1, startOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.Date;
            }
            else if (date.Between(startOfQuarter1.AddMonths(3), startOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.AddMonths(3).Date;
            }
            else if (date.Between(startOfQuarter1.AddMonths(6), startOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.AddMonths(6).Date;
            }

            return startOfQuarter1.AddMonths(9).Date;
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days from</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime date, TimeFrame timeFrame, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return timeFrame switch
            {
                TimeFrame.Day => 1,
                TimeFrame.Week => 7,
                TimeFrame.Month => culture.Calendar.GetDaysInMonth(date.Year, date.Month),
                TimeFrame.Quarter => date.EndOf(TimeFrame.Quarter, culture).DayOfYear - date.BeginningOf(TimeFrame.Quarter, culture).DayOfYear,
                _ => culture.Calendar.GetDaysInYear(date.Year),
            };
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days from</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo? culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
            {
                date.DaysIn(timeFrame, culture);
            }

            culture ??= CultureInfo.CurrentCulture;
            return date.EndOf(TimeFrame.Quarter, culture).DayOfYear - startOfQuarter1.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days left</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime date, TimeFrame timeFrame, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return timeFrame switch
            {
                TimeFrame.Day => 1,
                TimeFrame.Week => 7 - ((int)date.DayOfWeek + 1),
                TimeFrame.Month => date.DaysIn(TimeFrame.Month, culture) - date.Day,
                TimeFrame.Quarter => date.DaysIn(TimeFrame.Quarter, culture) - (date.DayOfYear - date.BeginningOf(TimeFrame.Quarter, culture).DayOfYear),
                _ => date.DaysIn(TimeFrame.Year, culture) - date.DayOfYear,
            };
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days left</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo? culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
            {
                return date.DaysLeftIn(timeFrame, culture);
            }

            culture ??= CultureInfo.CurrentCulture;
            return date.DaysIn(TimeFrame.Quarter, startOfQuarter1, culture) - (date.DayOfYear - startOfQuarter1.DayOfYear);
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime date, TimeFrame timeFrame, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            return timeFrame switch
            {
                TimeFrame.Day => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59),
                TimeFrame.Week => date.BeginningOf(TimeFrame.Week, culture).AddDays(6),
                TimeFrame.Month => date.AddMonths(1).BeginningOf(TimeFrame.Month, culture).AddDays(-1).Date,
                TimeFrame.Quarter => date.EndOf(TimeFrame.Quarter, date.BeginningOf(TimeFrame.Year, culture), culture),
                _ => new DateTime(date.Year, 12, 31),
            };
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo? culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
            {
                return date.EndOf(timeFrame, culture);
            }

            culture ??= CultureInfo.CurrentCulture;
            if (date.Between(startOfQuarter1, startOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.AddMonths(3).AddDays(-1).Date;
            }
            else if (date.Between(startOfQuarter1.AddMonths(3), startOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.AddMonths(6).AddDays(-1).Date;
            }
            else if (date.Between(startOfQuarter1.AddMonths(6), startOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, culture)))
            {
                return startOfQuarter1.AddMonths(9).AddDays(-1).Date;
            }

            return startOfQuarter1.AddYears(1).AddDays(-1).Date;
        }

        /// <summary>
        /// Determines if the date fulfills the comparison
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <param name="comparison">
        /// Comparison type (can be combined, so you can do weekday in the future, etc)
        /// </param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this DateTime date, DateCompare comparison)
        {
            return ((comparison & DateCompare.InFuture) != 0 && DateTime.Now < date)
                || ((comparison & DateCompare.InPast) != 0 && DateTime.Now > date)
                || ((comparison & DateCompare.Today) != 0 && DateTime.Today == date.Date)
                || ((comparison & DateCompare.WeekDay) != 0 && (int)date.DayOfWeek != 6 && date.DayOfWeek != 0)
                || ((comparison & DateCompare.WeekEnd) != 0 && (int)date.DayOfWeek == 6 || date.DayOfWeek == 0);
        }

        /// <summary>
        /// Gets the local time zone
        /// </summary>
        /// <param name="date">Date object</param>
        /// <returns>The local time zone</returns>
        public static TimeZoneInfo LocalTimeZone(this DateTime date) => TimeZoneInfo.Local;

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="hour">Hour to set</param>
        /// <param name="minutes">Minutes to set</param>
        /// <param name="seconds">Seconds to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime date, int hour, int minutes, int seconds) => date.SetTime(new TimeSpan(hour, minutes, seconds));

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="time">Time to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time) => date.Date.Add(time);

        /// <summary>
        /// Converts a DateTime to a specific time zone
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <param name="timeZone">Time zone to convert to</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime To(this DateTime date, TimeZoneInfo timeZone)
        {
            timeZone ??= TimeZoneInfo.Utc;
            return TimeZoneInfo.ConvertTime(date, timeZone);
        }

        /// <summary>
        /// Returns the date in int format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The date in Unix format</returns>
        public static int To(this DateTime date, DateTime epoch = default)
        {
            if (epoch == default)
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (int)((date.ToUniversalTime() - epoch).Ticks / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this int date, DateTime epoch = default)
        {
            if (epoch == default)
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return new DateTime((date * TimeSpan.TicksPerSecond) + epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this long date, DateTime epoch = default)
        {
            if (epoch == default)
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return new DateTime((date * TimeSpan.TicksPerSecond) + epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the DateTime object to string describing, relatively how long ago or how far in
        /// the future the input is based off of another DateTime object specified. ex: Input=March
        /// 21, 2013 Epoch=March 22, 2013 returns "1 day ago" Input=March 22, 2013 Epoch=March 21,
        /// 2013 returns "1 day from now"
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="epoch">DateTime object that the input is comparred to</param>
        /// <returns>The difference between the input and epoch expressed as a string</returns>
        public static string ToString(this DateTime input, DateTime epoch)
        {
            if (epoch == input)
                return "now";

            return epoch > input ? (epoch - input).ToStringFull() + " ago" : (input - epoch).ToStringFull() + " from now";
        }

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        /// <param name="date">Date to get the offset of</param>
        /// <returns>UTC offset</returns>
        public static double UTCOffset(this DateTime date) => (date - date.ToUniversalTime()).TotalHours;
    }
}