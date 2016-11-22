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
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Predicate extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PredicateExtensions
    {
        /// <summary>
        /// Adds the given values to the predicate set
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="predicate">Predicate to add to</param>
        /// <param name="values">Values to add</param>
        /// <returns>The resulting predicate set</returns>
        public static Predicate<T> AddToSet<T>(this Predicate<T> predicate, params T[] values)
        {
            if (predicate == null)
                return null;
            if (values == null || values.Length == 0)
                return predicate;
            return x => values.Contains(x) || predicate(x);
        }

        /// <summary>
        /// Treats the predicates as sets and does a cartesian product of them
        /// </summary>
        /// <typeparam name="T1">Type 1</typeparam>
        /// <typeparam name="T2">Type 2</typeparam>
        /// <param name="predicate1">Predicate 1</param>
        /// <param name="predicate2">Predicate 2</param>
        /// <returns>The cartesian product</returns>
        public static Func<T1, T2, bool> CartesianProduct<T1, T2>(this Predicate<T1> predicate1, Predicate<T2> predicate2)
        {
            if (predicate1 == null || predicate2 == null)
                return null;
            return (x, y) => predicate1(x) && predicate2(y);
        }

        /// <summary>
        /// Treats the predicates as sets and does a difference
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="predicate1">Predicate 1</param>
        /// <param name="predicate2">Predicate 2</param>
        /// <returns>The difference of the two predicates</returns>
        public static Predicate<T> Difference<T>(this Predicate<T> predicate1, Predicate<T> predicate2)
        {
            if (predicate1 == null)
                return null;
            if (predicate2 == null)
                return predicate1;
            return x => predicate1(x) ^ predicate2(x);
        }

        /// <summary>
        /// Treats predicates as sets and intersects them together
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="predicate1">Predicate 1</param>
        /// <param name="predicate2">Predicate 2</param>
        /// <returns>The intersected predicate</returns>
        public static Predicate<T> Intersect<T>(this Predicate<T> predicate1, Predicate<T> predicate2)
        {
            if (predicate1 == null || predicate2 == null)
                return null;
            return x => predicate1(x) && predicate2(x);
        }

        /// <summary>
        /// Treats predicates as sets and returns the relative complement
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="predicate1">Predicate 1</param>
        /// <param name="predicate2">Predicate 2</param>
        /// <returns>The relative complement</returns>
        public static Predicate<T> RelativeComplement<T>(this Predicate<T> predicate1, Predicate<T> predicate2)
        {
            if (predicate1 == null || predicate2 == null)
                return null;
            return x => predicate1(x) && !predicate2(x);
        }

        /// <summary>
        /// Removes the values from the predicate set
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <param name="values">Values to remove</param>
        /// <returns>The resulting set</returns>
        public static Predicate<T> RemoveFromSet<T>(this Predicate<T> predicate, params T[] values)
        {
            if (predicate == null)
                return null;
            if (values == null)
                return predicate;
            return x => !values.Contains(x) && predicate(x);
        }

        /// <summary>
        /// Treats predicates as sets and unions them together
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="predicate1">Predicate 1</param>
        /// <param name="predicate2">Predicate 2</param>
        /// <returns>The unioned predicate</returns>
        public static Predicate<T> Union<T>(this Predicate<T> predicate1, Predicate<T> predicate2)
        {
            if (predicate1 == null)
                return predicate2;
            if (predicate2 == null)
                return predicate1;
            return x => predicate1(x) || predicate2(x);
        }
    }
}