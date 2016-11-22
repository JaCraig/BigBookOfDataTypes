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
    /// ConcurrentBag extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ConcurrentBagExtensions
    {
        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ConcurrentBag<T> Add<T>(this ConcurrentBag<T> collection, IEnumerable<T> items)
        {
            collection = collection ?? new ConcurrentBag<T>();
            if (items == null)
                return collection;
            Parallel.ForEach(items, collection.Add);
            return collection;
        }

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ConcurrentBag<T> Add<T>(this ConcurrentBag<T> collection, params T[] items)
        {
            return collection.Add((IEnumerable<T>)items);
        }

        /// <summary>
        /// Adds an item to a list and returns the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="item">Item to add to the collection</param>
        /// <returns>The original item</returns>
        public static T AddAndReturn<T>(this ConcurrentBag<T> collection, T item)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            collection.Add(item);
            return item;
        }

        /// <summary>
        /// Adds items to the collection if it passes the predicate test
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if any are added, false otherwise</returns>
        public static bool AddIf<T>(this ConcurrentBag<T> collection, Predicate<T> predicate, params T[] items)
        {
            if (collection == null || predicate == null)
                return false;
            if (items == null || items.Length == 0)
                return true;
            return items.ForEachParallel(Item =>
            {
                if (predicate(Item))
                {
                    collection.Add(Item);
                    return true;
                }
                return false;
            }).Any(x => x);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIf<T>(this ConcurrentBag<T> collection, Predicate<T> predicate, IEnumerable<T> items)
        {
            return collection.AddIf(predicate, items.ToArray());
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, IEqualityComparer<T> comparer, params T[] items)
        {
            comparer = comparer ?? new GenericEqualityComparer<T>();
            return collection.AddIf(x => !collection.Contains(x, comparer), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, params T[] items)
        {
            return collection.AddIfUnique(new GenericEqualityComparer<T>(), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, Func<T, T, bool> predicate, params T[] items)
        {
            if (predicate == null)
                return false;
            return collection.AddIf(x => !collection.Any(y => predicate(x, y)), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="comparer">
        /// Equality comparer, if null then a generic equality comparer is used
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, IEqualityComparer<T> comparer, IEnumerable<T> items)
        {
            comparer = comparer ?? new GenericEqualityComparer<T>();
            return collection.AddIf(x => !collection.Contains(x, comparer), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, IEnumerable<T> items)
        {
            return collection.AddIfUnique(new GenericEqualityComparer<T>(), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ConcurrentBag<T> collection, Func<T, T, bool> predicate, IEnumerable<T> items)
        {
            if (predicate == null)
                return false;
            return collection.AddIf(x => !collection.Any(y => predicate(x, y)), items);
        }

        /// <summary>
        /// Determines whether the bag contains the item specified
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>True if the item is present, false otherwise</returns>
        public static bool Contains<T>(this ConcurrentBag<T> collection, T item, IEqualityComparer<T> comparer = null)
        {
            if (collection == null)
                return false;
            comparer = comparer ?? new GenericEqualityComparer<T>();
            foreach (T TempValue in collection)
            {
                if (comparer.Equals(TempValue, item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection to remove items from</param>
        /// <param name="predicate">Predicate used to determine what items to remove</param>
        public static ConcurrentBag<T> Remove<T>(this ConcurrentBag<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                return new ConcurrentBag<T>();
            if (predicate == null)
                return collection;
            return new ConcurrentBag<T>(collection.Where(x => !predicate(x)));
        }

        /// <summary>
        /// Removes all items in the list from the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to remove</param>
        /// <param name="comparer">
        /// Equality comparer, if null then a generic equality comparer is used.
        /// </param>
        /// <returns>The collection with the items removed</returns>
        public static ConcurrentBag<T> Remove<T>(this ConcurrentBag<T> collection, IEnumerable<T> items, IEqualityComparer<T> comparer = null)
        {
            if (collection == null)
                return new ConcurrentBag<T>();
            if (items == null)
                return collection;
            comparer = comparer ?? new GenericEqualityComparer<T>();
            return collection.Remove<T>(x => items.Contains(x, comparer));
        }
    }
}