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

using BigBook.Comparison;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BigBook
{
    /// <summary>
    /// IComparable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IComparableExtensions
    {
        /// <summary>
        /// Checks if an item is between two values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Value to check</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="comparer">Comparer used to compare the values (defaults to GenericComparer)"</param>
        /// <returns>True if it is between the values, false otherwise</returns>
        public static bool Between<T>(this T value, T min, T max, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(max, value) >= 0 && comparer.Compare(value, min) >= 0;
        }

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="value">Value sent in</param>
        /// <param name="max">Max value it can be (inclusive)</param>
        /// <param name="min">Min value it can be (inclusive)</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The value set between Min and Max</returns>
        public static T Clamp<T>(this T value, T max, T min, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            if (comparer.Compare(max, value) < 0)
                return max;
            if (comparer.Compare(value, min) < 0)
                return min;
            return value;
        }

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="inputA">Input A</param>
        /// <param name="inputB">Input B</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The maximum value</returns>
        public static T Max<T>(this T inputA, T inputB, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(inputA, inputB) < 0 ? inputB : inputA;
        }

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="inputA">Input A</param>
        /// <param name="inputB">Input B</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The minimum value</returns>
        public static T Min<T>(this T inputA, T inputB, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(inputA, inputB) > 0 ? inputB : inputA;
        }
    }
}