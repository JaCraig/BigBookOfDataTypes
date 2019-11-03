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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Extension methods that add basic math functions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MathExtensions
    {
        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static decimal Absolute(this decimal value) => Math.Abs(value);

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static double Absolute(this double value) => Math.Abs(value);

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static float Absolute(this float value) => Math.Abs(value);

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static int Absolute(this int value)
        {
            if (value == int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be int.MinValue");
            }

            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static long Absolute(this long value)
        {
            if (value == -9223372036854775808)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be -9223372036854775808");
            }

            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static short Absolute(this short value)
        {
            if (value == -32768)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be -32768");
            }

            return Math.Abs(value);
        }

        /// <summary>
        /// Returns E raised to the specified power
        /// </summary>
        /// <param name="value">Power to raise E by</param>
        /// <returns>E raised to the specified power</returns>
        public static double Exp(this double value) => Math.Exp(value);

        /// <summary>
        /// Calculates the factorial for a number
        /// </summary>
        /// <param name="input">Input value (N!)</param>
        /// <returns>The factorial specified</returns>
        public static int Factorial(this int input)
        {
            var Value1 = 1;
            for (var x = 2; x <= input; ++x)
            {
                Value1 *= x;
            }

            return Value1;
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this int value1, int value2)
        {
            if (value1 == int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be int.MinValue");
            }

            if (value2 == int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be int.MinValue");
            }

            value1 = value1.Absolute();
            value2 = value2.Absolute();
            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                {
                    value1 %= value2;
                }
                else
                {
                    value2 %= value1;
                }
            }
            return value1 == 0 ? value2 : value1;
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this int value1, uint value2)
        {
            if (value1 == int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be int.MinValue");
            }

            if (value2 == 2147483648)
            {
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be 2147483648");
            }

            return value1.GreatestCommonDenominator((int)value2);
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this uint value1, uint value2)
        {
            if (value1 == 2147483648)
            {
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be 2147483648");
            }

            if (value2 == 2147483648)
            {
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be 2147483648");
            }

            return ((int)value1).GreatestCommonDenominator((int)value2);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number
        /// </summary>
        /// <param name="value">Specified number</param>
        /// <returns>The natural logarithm of the specified number</returns>
        public static double Log(this double value) => Math.Log(value);

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="baseValue">Base</param>
        /// <returns>The logarithm of a specified number in a specified base</returns>
        public static double Log(this double value, double baseValue) => Math.Log(value, baseValue);

        /// <summary>
        /// Returns the base 10 logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The base 10 logarithm of the specified number</returns>
        public static double Log10(this double value) => Math.Log10(value);

        /// <summary>
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="values">The list of values</param>
        /// <param name="average">
        /// Function used to find the average of two values if the number of values is even.
        /// </param>
        /// <param name="orderBy">Function used to order the values</param>
        /// <returns>The median value</returns>
        public static T Median<T>(this IEnumerable<T> values, Func<T, T, T> average = null, Func<T, T> orderBy = null)
        {
            if (values == null)
            {
                return default(T);
            }

            if (!values.Any())
            {
                return default(T);
            }

            average = average ?? ((x, _) => x);
            orderBy = orderBy ?? (x => x);
            values = values.OrderBy(orderBy);
            if (values.Count() % 2 == 0)
            {
                var Element1 = values.ElementAt(values.Count() / 2);
                var Element2 = values.ElementAt((values.Count() / 2) - 1);
                return average(Element1, Element2);
            }
            return values.ElementAt(values.Count() / 2);
        }

        /// <summary>
        /// Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="values">The list of values</param>
        /// <returns>The mode value</returns>
        public static T Mode<T>(this IEnumerable<T> values)
        {
            if (values == null)
            {
                return default(T);
            }

            if (!values.Any())
            {
                return default(T);
            }

            var Items = new Bag<T>();
            foreach (var Value in values)
            {
                Items.Add(Value);
            }

            var MaxValue = 0;
            var MaxIndex = default(T);
            foreach (var Key in Items)
            {
                if (Items[Key] > MaxValue)
                {
                    MaxValue = Items[Key];
                    MaxIndex = Key;
                }
            }
            return MaxIndex;
        }

        /// <summary>
        /// Raises Value to the power of Power
        /// </summary>
        /// <param name="value">Value to raise</param>
        /// <param name="power">Power</param>
        /// <returns>The resulting value</returns>
        public static double Pow(this double value, double power) => Math.Pow(value, power);

        /// <summary>
        /// Raises Value to the power of Power
        /// </summary>
        /// <param name="value">Value to raise</param>
        /// <param name="power">Power</param>
        /// <returns>The resulting value</returns>
        public static double Pow(this decimal value, decimal power) => Math.Pow((double)value, (double)power);

        /// <summary>
        /// Rounds the value to the number of digits
        /// </summary>
        /// <param name="value">Value to round</param>
        /// <param name="digits">Digits to round to</param>
        /// <param name="rounding">Rounding mode to use</param>
        /// <returns></returns>
        public static double Round(this double value, int digits = 2, MidpointRounding rounding = MidpointRounding.AwayFromZero)
        {
            if (digits < 0)
            {
                digits = 0;
            }

            if (digits > 15)
            {
                digits = 15;
            }

            return Math.Round(value, digits, rounding);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this double value) => Math.Sqrt(value);

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this float value) => Math.Sqrt(value);

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this int value) => Math.Sqrt(value);

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this long value) => Math.Sqrt(value);

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this short value) => Math.Sqrt(value);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<double> values) => values.StandardDeviation(x => x);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<decimal> values) => values.StandardDeviation(x => x);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<float> values) => values.StandardDeviation(x => x);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<long> values) => values.StandardDeviation(x => x);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<int> values) => values.StandardDeviation(x => x);

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, double> selector = null) => values.Variance(selector).Sqrt();

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, decimal> selector) => values.Variance(selector).Sqrt();

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, float> selector) => values.Variance(selector).Sqrt();

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, long> selector) => values.Variance(selector).Sqrt();

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, int> selector) => values.Variance(selector).Sqrt();

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<double> values) => values.Variance(x => x);

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<int> values) => values.Variance(x => x);

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<long> values) => values.Variance(x => x);

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<decimal> values) => values.Variance(x => (double)x);

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<float> values) => values.Variance(x => x);

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The variance</returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, double> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return values.Variance(x => (decimal)selector(x));
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The variance</returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, int> selector) => values.Variance(x => (decimal)selector(x));

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The variance</returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, long> selector) => values.Variance(x => (decimal)selector(x));

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The variance</returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, float> selector) => values.Variance(x => (decimal)selector(x));

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>The variance</returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, decimal> selector)
        {
            if (values?.Any() != true)
            {
                return 0;
            }

            var MeanValue = values.Average(selector);
            var Sum = values.Sum(x => (selector(x) - MeanValue).Pow(2));
            return Sum / values.Count();
        }
    }
}