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
using System.Collections.Concurrent;
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
        /// Constructor
        /// </summary>
        public Cache()
        {
            InternalCache = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public override int Count => InternalCache?.Count ?? 0;

        /// <summary>
        /// Keys
        /// </summary>
        public override ICollection<string> Keys => InternalCache?.Keys ?? Array.Empty<string>();

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get; } = "Default";

        /// <summary>
        /// Values
        /// </summary>
        public override ICollection<object> Values => InternalCache?.Values ?? Array.Empty<object>();

        /// <summary>
        /// Internal cache
        /// </summary>
        protected ConcurrentDictionary<string, object>? InternalCache { get; private set; }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public override void Add(string key, object value) => InternalCache?.AddOrUpdate(key, _ => value, (_, __) => value);

        /// <summary>
        /// Clears the cache
        /// </summary>
        public override void Clear() => InternalCache?.Clear();

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public override bool Contains(KeyValuePair<string, object> item) => InternalCache.Contains(item);

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public override bool ContainsKey(string key) => InternalCache?.ContainsKey(key) ?? false;

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => InternalCache?.ToArray().CopyTo(array, arrayIndex);

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator() => (InternalCache ?? new ConcurrentDictionary<string, object>()).GetEnumerator();

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public override bool Remove(string key) => InternalCache?.TryRemove(key, out _) ?? false;

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public override bool Remove(KeyValuePair<string, object> item) => InternalCache?.TryRemove(item.Key, out _) ?? false;

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public override bool TryGetValue(string key, out object value)
        {
            value = 0;
            return InternalCache?.TryGetValue(key, out value) ?? false;
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        /// <param name="Managed">Managed objects or just unmanaged</param>
        protected override void Dispose(bool Managed)
        {
            if (InternalCache != null)
            {
                foreach (var Item in InternalCache.Values.OfType<IDisposable>())
                {
                    Item.Dispose();
                }
                InternalCache.Clear();
                InternalCache = null;
            }
        }
    }
}