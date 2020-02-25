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
    /// ICollection extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> collection, IEnumerable<T> items) => collection.Add(items.ToArray());

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection is null)
                return new List<T>();

            if (items is null)
                return collection;

            items.ForEach(x => collection.Add(x));
            return collection;
        }

        /// <summary>
        /// Adds an item to a list and returns the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="item">Item to add to the collection</param>
        /// <returns>The original item</returns>
        public static T AddAndReturn<T>(this ICollection<T> collection, T item)
        {
            if (collection is null)
                return item;

            collection.Add(item);
            return item;
        }

        /// <summary>
        /// Adds items to the collection if it passes the predicate test
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if any are added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> collection, Predicate<T> predicate, params T[] items)
        {
            if (collection is null || predicate is null)
                return false;

            if (items is null || items.Length == 0)
                return true;

            var ReturnValue = false;
            for (int x = 0, itemsLength = items.Length; x < itemsLength; x++)
            {
                var Item = items[x];
                if (predicate(Item))
                {
                    collection.Add(Item);
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> collection, Predicate<T> predicate, IEnumerable<T> items)
        {
            return !(collection is null)
                && !(predicate is null)
                && (items?.Any() != true
                    || collection.AddIf(predicate, items.ToArray()));
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, params T[] items)
        {
            return !(collection is null)
                && (items is null
                    || collection.AddIf(x => !collection.Contains(x), items));
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, Func<T, T, bool> predicate, params T[] items)
        {
            return !(collection is null)
                && !(predicate is null)
                && (items is null
                    || collection.AddIf(x => !collection.Any(y => predicate(x, y)), items));
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            return !(collection is null)
                && (items is null
                    || collection.AddIf(x => !collection.Contains(x), items));
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, Func<T, T, bool> predicate, IEnumerable<T> items)
        {
            return !(collection is null)
                && !(predicate is null)
                && (items is null
                    || collection.AddIf(x => !collection.Any(y => predicate(x, y)), items));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">0 based item to start with (inclusive)</param>
        /// <param name="end">0 based item to end with (exclusive)</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IList<T> For<T>(this IList<T> list, int start, int end, Action<T, int> action)
        {
            if (list is null)
                return new List<T>();

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
        /// <typeparam name="TObject">Object type</typeparam>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">0 based item to start with (inclusive)</param>
        /// <param name="end">0 based item to end with (exclusive)</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IList<TReturn> For<TObject, TReturn>(this IList<TObject> list, int start, int end, Func<TObject, int, TReturn> function)
        {
            if (list is null || function is null)
                return new List<TReturn>();

            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            var ReturnList = new List<TReturn>();
            for (var x = 0; x < TempList.Length; ++x)
            {
                ReturnList.Add(function(TempList[x], x));
            }
            return ReturnList;
        }

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection to remove items from</param>
        /// <param name="predicate">Predicate used to determine what items to remove</param>
        public static ICollection<T> Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            return collection?.Where(x => !predicate(x)).ToList() ?? new List<T>();
        }

        /// <summary>
        /// Removes all items in the list from the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to remove</param>
        /// <returns>The collection with the items removed</returns>
        public static ICollection<T> Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is null)
                return new List<T>();

            if (items is null)
                return collection;

            return collection.Where(x => !items.Contains(x)).ToList();
        }
    }
}