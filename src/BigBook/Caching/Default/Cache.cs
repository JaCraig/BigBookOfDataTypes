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

using BigBook.Caching.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.Caching.Default
{
    /// <summary>
    /// Built in cache
    /// </summary>
    public class Cache : CacheBase
    {
        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public override int Count => InternalCache.Count;

        /// <summary>
        /// Keys
        /// </summary>
        public override ICollection<string> Keys => InternalCache.Keys;

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get; } = "Default";

        /// <summary>
        /// Values
        /// </summary>
        public override ICollection<object> Values => InternalCache.Values;

        /// <summary>
        /// Internal cache
        /// </summary>
        protected Dictionary<string, object> InternalCache { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public override bool Contains(KeyValuePair<string, object> item)
        {
            return InternalCache.Contains(item);
        }

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public override bool ContainsKey(string key)
        {
            return InternalCache.ContainsKey(key);
        }

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            InternalCache.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return InternalCache.GetEnumerator();
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        /// <param name="Managed">Managed objects or just unmanaged</param>
        protected override void Dispose(bool Managed)
        {
            if (InternalCache is null)
                return;
            foreach (var Item in InternalCache.Values.OfType<IDisposable>())
            {
                Item.Dispose();
            }
            InternalCache.Clear();
        }

        /// <summary>
        /// Used internally to add items (a lock is already placed by this point in time).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void InternalAdd(string key, object value)
        {
            if (InternalCache.TryGetValue(key, out _))
                InternalCache[key] = value;
            else
                InternalCache.Add(key, value);
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        protected override void InternalClear()
        {
            InternalCache.Clear();
        }

        /// <summary>
        /// The internal method called to remove an item. (a lock has already been placed by this point)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if it is removed, false otherwise.</returns>
        protected override bool InternalRemove(string key)
        {
            return InternalCache.Remove(key);
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        protected override bool InternalTryGetValue(string key, out object value)
        {
            return InternalCache.TryGetValue(key, out value);
        }
    }
}