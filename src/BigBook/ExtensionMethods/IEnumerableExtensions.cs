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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BigBook
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Combines multiple IEnumerables together and returns a new IEnumerable containing all of
        /// the values
        /// </summary>
        /// <typeparam name="T">Type of the data in the IEnumerable</typeparam>
        /// <param name="enumerable1">IEnumerable 1</param>
        /// <param name="additions">IEnumerables to concat onto the first item</param>
        /// <returns>A new IEnumerable containing all values</returns>
        /// <example>
        /// <code>
        ///int[] TestObject1 = new int[] { 1, 2, 3 }; int[] TestObject2 = new int[] { 4, 5, 6
        ///}; int[] TestObject3 = new int[] { 7, 8, 9 }; TestObject1 =
        ///TestObject1.Concat(TestObject2, TestObject3).ToArray();
        /// </code>
        /// </example>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable1, params IEnumerable<T>[] additions)
        {
            enumerable1 ??= Array.Empty<T>();
            if (additions is null)
            {
                return enumerable1;
            }

            var ActualAdditions = additions.Where(x => !(x is null)).ToArray();
            var Results = new List<T>();
            Results.AddRange(enumerable1);
            for (var x = 0; x < ActualAdditions.Length; ++x)
            {
                Results.AddRange(ActualAdditions[x]);
            }

            return Results;
        }

        /// <summary>
        /// Returns only distinct items from the IEnumerable based on the predicate
        /// </summary>
        /// <typeparam name="T">Object type within the list</typeparam>
        /// <param name="enumerable">List of objects</param>
        /// <param name="predicate">
        /// Predicate that is used to determine if two objects are equal. True if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>An IEnumerable of only the distinct items</returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, Func<T, T, bool> predicate)
        {
            if (enumerable?.Any() != true)
            {
                yield break;
            }

            var TempGenericComparer = new GenericEqualityComparer<T>();
            predicate ??= TempGenericComparer.Equals;
            var Results = new List<T>();
            foreach (var Item in enumerable)
            {
                if (!Results.Any(x => predicate(Item, x)))
                {
                    Results.Add(Item);
                    yield return Item;
                }
            }
        }

        /// <summary>
        /// Returns elements starting at the index and ending at the end index
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List to search</param>
        /// <param name="start">Start index (inclusive)</param>
        /// <param name="end">End index (exclusive)</param>
        /// <returns>The items between the start and end index</returns>
        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> list, int start, int end)
        {
            if (list?.Any() != true)
            {
                yield break;
            }

            if (end > list.Count())
            {
                end = list.Count();
            }

            if (start < 0)
            {
                start = 0;
            }

            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempList = list.ToArray();
            for (var x = start; x < end; ++x)
            {
                yield return TempList[x];
            }
        }

        /// <summary>
        /// Removes values from a list that meet the criteria set forth by the predicate
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">List to cull items from</param>
        /// <param name="predicate">Predicate that determines what items to remove</param>
        /// <returns>An IEnumerable with the objects that meet the criteria removed</returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> value, Func<T, bool> predicate)
        {
            if (value?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (predicate is null)
            {
                return value;
            }

            return value.Where(x => !predicate(x));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with (inclusive)</param>
        /// <param name="end">Item to end with (exclusive)</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> list, int start, int end, Action<T, int> action)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            for (var x = 0; x < TempList.Length; ++x)
            {
                action(TempList[x], x);
            }
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and
        /// returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> list, int start, int end, Func<T, int, R> function)
        {
            if (list is null || function is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            var ReturnList = new R[TempList.Length];
            for (var x = 0; x < TempList.Length; ++x)
            {
                ReturnList[x] = function(TempList[x], x);
            }
            return ReturnList;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (action is null)
            {
                return list;
            }

            foreach (var Item in list)
            {
                action(Item);
            }

            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            if (list is null || function is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            var ReturnList = new List<R>(list.Count());
            foreach (var Item in list)
            {
                ReturnList.Add(function(Item));
            }

            return ReturnList;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action, Action<T, Exception> catchAction)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (action is null || catchAction is null)
            {
                return list;
            }

            foreach (var Item in list)
            {
                try
                {
                    action(Item);
                }
                catch (Exception e) { catchAction(Item, e); }
            }
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> list, Func<T, R> function, Action<T, Exception> catchAction)
        {
            if (list is null || function is null || catchAction is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            var ReturnValue = new List<R>();
            foreach (var Item in list)
            {
                try
                {
                    ReturnValue.Add(function(Item));
                }
                catch (Exception e) { catchAction(Item, e); }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (action is null)
            {
                return list;
            }

            Parallel.ForEach(list, action);
            return list;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            if (list is null || function is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            return list.ForParallel(0, list.Count() - 1, (x, _) => function(x));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> list, Action<T> action, Action<T, Exception> catchAction)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (action is null || catchAction is null)
            {
                return list;
            }

            Parallel.ForEach<T>(list, (T Item) =>
            {
                try
                {
                    action(Item);
                }
                catch (Exception e) { catchAction(Item, e); }
            });
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> list, Func<T, R> function, Action<T, Exception> catchAction)
        {
            if (list is null || function is null || catchAction is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            var ReturnValues = new ConcurrentBag<R>();
            Parallel.ForEach<T>(list, (T Item) =>
            {
                try
                {
                    ReturnValues.Add(function(Item));
                }
                catch (Exception e) { catchAction(Item, e); }
            });
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> list, int start, int end, Action<T, int> action)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (action is null)
            {
                return list;
            }

            if (end >= list.Count())
            {
                end = list.Count() - 1;
            }

            if (start < 0)
            {
                start = 0;
            }

            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempArray = list.ToArray();
            Parallel.For(start, end + 1, new Action<int>(x => action(TempArray[x], x)));
            return list;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> list, int start, int end, Func<T, int, R> function)
        {
            if (list is null || function is null || !list.Any())
            {
                return Array.Empty<R>();
            }

            if (end >= list.Count())
            {
                end = list.Count() - 1;
            }

            if (start < 0)
            {
                start = 0;
            }

            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempArray = list.ToArray();
            var Results = new R[(end + 1) - start];
            Parallel.For(start, end + 1, new Action<int>(x => Results[x - start] = function(TempArray[x], x)));
            return Results;
        }

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="count">Number of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>(this IEnumerable<T> list, int count)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            return list.ElementsBetween(list.Count() - count, list.Count());
        }

        /// <summary>
        /// Does a left join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a left join of the two lists</returns>
        public static IEnumerable<R> LeftJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key>? comparer = null)
        {
            if (inner is null
                || outerKeySelector is null
                || innerKeySelector is null
                || resultSelector is null)
            {
                return Array.Empty<R>();
            }

            comparer ??= new GenericEqualityComparer<Key>();
            return outer.ForEach(x => new { left = x, right = inner.FirstOrDefault(y => comparer.Equals(innerKeySelector(y), outerKeySelector(x))) })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Does an outer join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns an outer join of the two lists</returns>
        public static IEnumerable<R> OuterJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key>? comparer = null)
        {
            if (inner is null
                || outer is null
                || outerKeySelector is null
                || innerKeySelector is null
                || resultSelector is null)
            {
                return Array.Empty<R>();
            }

            var Left = outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            var Right = outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            return Left.Union(Right);
        }

        /// <summary>
        /// Determines the position of an object if it is present, otherwise it returns -1
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List of objects to search</param>
        /// <param name="item">Object to find the position of</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is present
        /// </param>
        /// <returns>The position of the object if it is present, otherwise -1</returns>
        public static int PositionOf<T>(this IEnumerable<T> list, T item, IEqualityComparer<T>? equalityComparer = null)
        {
            if (list?.Any() != true)
            {
                return -1;
            }

            equalityComparer ??= new GenericEqualityComparer<T>();
            var Count = 0;
            foreach (var TempItem in list)
            {
                if (equalityComparer.Equals(item, TempItem))
                {
                    return Count;
                }

                ++Count;
            }
            return -1;
        }

        /// <summary>
        /// Does a right join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a right join of the two lists</returns>
        public static IEnumerable<R> RightJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key>? comparer = null)
        {
            if (outer is null
                || outerKeySelector is null
                || innerKeySelector is null
                || resultSelector is null)
            {
                return Array.Empty<R>();
            }

            comparer ??= new GenericEqualityComparer<Key>();
            return inner.ForEach(x => new { left = outer.FirstOrDefault(y => comparer.Equals(innerKeySelector(x), outerKeySelector(y))), right = x })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> list, Func<T, bool> predicate, Func<Exception> exception)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (predicate is null || exception is null)
            {
                return list;
            }

            if (list.All(predicate))
            {
                throw exception();
            }

            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception exception)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (predicate is null || exception is null)
            {
                return list;
            }

            if (list.All(predicate))
            {
                throw exception;
            }

            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> list, Func<T, bool> predicate, Func<Exception> exception)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (predicate is null || exception is null)
            {
                return list;
            }

            if (list.Any(predicate))
            {
                throw exception();
            }

            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception exception)
        {
            if (list?.Any() != true)
            {
                return Array.Empty<T>();
            }

            if (predicate is null || exception is null)
            {
                return list;
            }

            if (list.Any(predicate))
            {
                throw exception;
            }

            return list;
        }

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="convertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> list, Func<Source, Target> convertingFunction)
        {
            if (list is null || convertingFunction is null || !list.Any())
            {
                return Array.Empty<Target>();
            }

            return list.ForEach(convertingFunction).ToArray();
        }

        /// <summary>
        /// Converts an IEnumerable to a list
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="list">IEnumerable to convert</param>
        /// <param name="convertingFunction">Function used to convert each item</param>
        /// <returns>The list containing the items from the IEnumerable</returns>
        public static List<Target> ToList<Source, Target>(this IEnumerable<Source> list, Func<Source, Target> convertingFunction)
        {
            if (list is null || convertingFunction is null || !list.Any())
            {
                return new List<Target>();
            }

            return list.ForEach(convertingFunction).ToList();
        }

        /// <summary>
        /// Converts the IEnumerable to an observable list
        /// </summary>
        /// <typeparam name="Source">The type of the source.</typeparam>
        /// <typeparam name="Target">The type of the target.</typeparam>
        /// <param name="list">The list to convert</param>
        /// <param name="convertingFunction">The converting function.</param>
        /// <returns>The observable list version of the original list</returns>
        public static ObservableList<Target> ToObservableList<Source, Target>(this IEnumerable<Source> list, Func<Source, Target> convertingFunction)
        {
            if (list is null)
            {
                return new ObservableList<Target>();
            }

            convertingFunction ??= new Func<Source, Target>(x => x.To<Source, Target>());
            return new ObservableList<Target>(list.ForEach(convertingFunction));
        }

        /// <summary>
        /// Converts the IEnumerable to an observable list
        /// </summary>
        /// <typeparam name="Source">The type of the source.</typeparam>
        /// <param name="list">The list to convert</param>
        /// <returns>The observable list version of the original list</returns>
        public static ObservableList<Source> ToObservableList<Source>(this IEnumerable<Source> list)
        {
            if (list is null)
            {
                return new ObservableList<Source>();
            }

            return new ObservableList<Source>(list);
        }

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="itemOutput">
        /// Used to convert the item to a string (defaults to calling ToString)
        /// </param>
        /// <param name="seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> list, Func<T, string>? itemOutput = null, string seperator = ",")
        {
            if (list?.Any() != true)
            {
                return "";
            }

            seperator ??= "";
            itemOutput ??= (x => x?.ToString() ?? "");
            return string.Join(seperator, list.Select(itemOutput));
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="collection">The collection hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> property)
        {
            if (collection?.Any() != true)
            {
                yield break;
            }

            foreach (var item in collection)
            {
                yield return item;
                foreach (var inner in Transverse(property(item), property))
                {
                    yield return inner;
                }
            }
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="item">The root node of the hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this T item, Func<T, IEnumerable<T>> property)
        {
            if (Equals(item, default(T)!))
            {
                yield break;
            }

            yield return item;
            foreach (var inner in Transverse(property(item), property))
            {
                yield return inner;
            }
        }
    }
}