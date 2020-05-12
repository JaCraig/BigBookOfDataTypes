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

using BigBook.Caching.Interfaces;
using BigBook.Patterns.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.Caching.BaseClasses
{
    /// <summary>
    /// Cache base class
    /// </summary>
    public abstract class CacheBase : SafeDisposableBaseClass, ICache
    {
        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Keys
        /// </summary>
        public abstract ICollection<string> Keys { get; }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public IEnumerable<int> Tags => TagMappings.First;

        /// <summary>
        /// Values
        /// </summary>
        public abstract ICollection<object> Values { get; }

        /// <summary>
        /// Tag mappings
        /// </summary>
        protected ManyToManyIndex<int, string> TagMappings { get; } = new ManyToManyIndex<int, string>();

        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object LockObject = new object();

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The object specified</returns>
        public object this[string key]
        {
            get
            {
                lock (LockObject)
                {
                    InternalTryGetValue(key, out var Value);
                    return Value;
                }
            }
            set
            {
                lock (LockObject)
                {
                    InternalAdd(key, value);
                }
            }
        }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public void Add(string key, object value)
        {
            lock (LockObject)
            {
                InternalAdd(key, value);
            }
        }

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<string, object> item)
        {
            lock (LockObject)
            {
                InternalAdd(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Adds a value/key combination and assigns tags to it
        /// </summary>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to add</param>
        /// <param name="tags">Tags to associate with the key/value pair</param>
        public void Add(string key, object value, IEnumerable<string> tags)
        {
            tags ??= Array.Empty<string>();
            lock (LockObject)
            {
                InternalAdd(key, value);
                TagMappings.Add(key, tags.Select(tag => tag.GetHashCode(StringComparison.Ordinal)));
            }
        }

        /// <summary>
        /// Adds a value/key combination and assigns tags to it
        /// </summary>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to add</param>
        /// <param name="tags">Tags to associate with the key/value pair</param>
        public void Add(string key, object value, params string[] tags)
        {
            tags ??= Array.Empty<string>();
            lock (LockObject)
            {
                InternalAdd(key, value);
                TagMappings.Add(key, tags.Select(tag => tag.GetHashCode(StringComparison.Ordinal)));
            }
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            lock (LockObject)
            {
                TagMappings.Clear();
                InternalClear();
            }
        }

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public abstract bool Contains(KeyValuePair<string, object> item);

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public abstract bool ContainsKey(string key);

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public abstract void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex);

        /// <summary>
        /// Gets the objects associated with a specific tag
        /// </summary>
        /// <param name="tag">Tag to use</param>
        /// <returns>The objects associated with the tag</returns>
        public IEnumerable<object> GetByTag(string tag)
        {
            if (tag is null || !TagMappings.TryGetValue(tag.GetHashCode(StringComparison.Ordinal), out var Keys))
                yield break;

            foreach (var Key in Keys)
            {
                if (InternalTryGetValue(Key, out var ReturnValue))
                    yield return ReturnValue;
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(string key)
        {
            lock (LockObject)
            {
                TagMappings.Remove(key);
                return InternalRemove(key);
            }
        }

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            lock (LockObject)
            {
                var Key = item.Key;
                TagMappings.Remove(Key);
                return InternalRemove(Key);
            }
        }

        /// <summary>
        /// Removes all items associated with the tag specified
        /// </summary>
        /// <param name="tag">Tag to remove</param>
        public void RemoveByTag(string tag)
        {
            if (tag is null)
                return;
            var TagHashCode = tag.GetHashCode(StringComparison.Ordinal);
            if (!TagMappings.TryGetValue(TagHashCode, out var Keys))
                return;
            lock (LockObject)
            {
                foreach (var Key in Keys)
                {
                    InternalRemove(Key);
                }
                TagMappings.Remove(TagHashCode);
            }
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool TryGetValue(string key, out object value)
        {
            lock (LockObject)
            {
                return InternalTryGetValue(key, out value);
            }
        }

        /// <summary>
        /// Used internally to add items (a lock is already placed by this point in time).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void InternalAdd(string key, object value);

        /// <summary>
        /// Clear internal method (lock has already been placed)
        /// </summary>
        protected abstract void InternalClear();

        /// <summary>
        /// The internal method called to remove an item. (a lock has already been placed by this point)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if it is removed, false otherwise.</returns>
        protected abstract bool InternalRemove(string key);

        /// <summary>
        /// Attempts to get a value.(Lock is already placed)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is successful, false otherwise.</returns>
        protected abstract bool InternalTryGetValue(string key, out object value);
    }
}