﻿/*
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
using System.Globalization;

namespace BigBook
{
    /// <summary>
    /// Represents a date span
    /// </summary>
    public class DateSpan
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Start of the date span</param>
        /// <param name="end">End of the date span</param>
        public DateSpan(DateTime start, DateTime end)
        {
            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            Start = start;
            End = end;
            var Diff = End - Start;
            Days = Diff.DaysRemainder();
            Hours = Diff.Hours;
            MilliSeconds = Diff.Milliseconds;
            Minutes = Diff.Minutes;
            Months = Diff.Months();
            Seconds = Diff.Seconds;
            Years = Diff.Years();
        }

        /// <summary>
        /// Days between the two dates
        /// </summary>
        public int Days { get; }

        /// <summary>
        /// End date
        /// </summary>
        public DateTime End { get; }

        /// <summary>
        /// Hours between the two dates
        /// </summary>
        public int Hours { get; }

        /// <summary>
        /// Milliseconds between the two dates
        /// </summary>
        public int MilliSeconds { get; }

        /// <summary>
        /// Minutes between the two dates
        /// </summary>
        public int Minutes { get; }

        /// <summary>
        /// Months between the two dates
        /// </summary>
        public int Months { get; }

        /// <summary>
        /// Seconds between the two dates
        /// </summary>
        public int Seconds { get; }

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime Start { get; }

        /// <summary>
        /// Years between the two dates
        /// </summary>
        public int Years { get; }

        /// <summary>
        /// Adds the specified values.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result.</returns>
        public static DateSpan? Add(DateSpan? left, DateSpan? right) => left + right;

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(DateSpan? value)
        {
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Determines if two DateSpans are not equal
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DateSpan? span1, DateSpan? span2)
        {
            return !(span1 == span2);
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>The combined date span</returns>
        public static DateSpan? operator +(DateSpan? span1, DateSpan? span2)
        {
            if (span1 is null)
            {
                return span2 is null ? null : new DateSpan(span2.Start, span2.End);
            }
            else if (span2 is null)
            {
                return new DateSpan(span1.Start, span1.End);
            }

            var Start = span1.Start < span2.Start ? span1.Start : span2.Start;
            var End = span1.End > span2.End ? span1.End : span2.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans are equal
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(DateSpan? span1, DateSpan? span2)
        {
            return (span1 is null && span2 is null)
                || (!(span1 is null)
                    && !(span2 is null)
                    && span1.Start == span2.Start
                    && span1.End == span2.End);
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is DateSpan Tempobj
                && Tempobj == this;
        }

        /// <summary>
        /// Gets the hash code for the date span
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => End.GetHashCode() & Start.GetHashCode();

        /// <summary>
        /// Returns the intersecting time span between the two values
        /// </summary>
        /// <param name="span">Span to use</param>
        /// <returns>The intersection of the two time spans</returns>
        public DateSpan? Intersection(DateSpan? span)
        {
            if (span is null || !Overlap(span))
            {
                return null;
            }

            var TempStart = span.Start > Start ? span.Start : Start;
            var TempEnd = span.End < End ? span.End : End;
            return new DateSpan(TempStart, TempEnd);
        }

        /// <summary>
        /// Determines if two DateSpans overlap
        /// </summary>
        /// <param name="span">The span to compare to</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap(DateSpan? span)
        {
            return !(span is null)
                && ((Start >= span.Start && Start < span.End)
                    || (End <= span.End && End > span.Start)
                    || (Start <= span.Start && End >= span.End));
        }

        /// <summary>
        /// Converts the DateSpan to a string
        /// </summary>
        /// <returns>The DateSpan as a string</returns>
        public override string ToString() => $"Start: {Start.ToString(CultureInfo.InvariantCulture)} End: {End.ToString(CultureInfo.InvariantCulture)}";
    }
}