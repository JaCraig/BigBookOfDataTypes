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
using System.Collections.Generic;

namespace BigBook.Caching.BaseClasses
{
    /// <summary>
    /// Cache base class
    /// </summary>
    public abstract class CacheBase : SafeDisposableBaseClass, ICache
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected CacheBase()
        {
            TagMappings = new ListMapping<string, string>();
        }

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
        /// The tags used thus far
        /// </summary>
        public IEnumerable<string> Tags => TagMappings.Keys;

        /// <summary>
        /// Values
        /// </summary>
        public abstract ICollection<object> Values { get; }

        /// <summary>
        /// Tag mappings
        /// </summary>
        protected ListMapping<string, string> TagMappings { get; }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The object specified</returns>
        public object this[string key]
        {
            get
            {
                TryGetValue(key, out object Value);
                return Value;
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public abstract void Add(string key, object value);

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds a value/key combination and assigns tags to it
        /// </summary>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to add</param>
        /// <param name="tags">Tags to associate with the key/value pair</param>
        public void Add(string key, object value, IEnumerable<string> tags)
        {
            Add(key, value);
            tags.ForEach(x => TagMappings.Add(x, key));
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public abstract void Clear();

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
            var ReturnValue = new List<object>();
            if (!TagMappings.ContainsKey(tag))
            {
                return ReturnValue;
            }

            foreach (string Key in TagMappings[tag])
            {
                if (ContainsKey(Key))
                {
                    ReturnValue.Add(this[Key]);
                }
            }
            return ReturnValue;
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
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public abstract bool Remove(string key);

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public abstract bool Remove(KeyValuePair<string, object> item);

        /// <summary>
        /// Removes all items associated with the tag specified
        /// </summary>
        /// <param name="tag">Tag to remove</param>
        public void RemoveByTag(string tag)
        {
            if (!TagMappings.ContainsKey(tag))
            {
                return;
            }

            TagMappings[tag].ForEach(Remove);
            TagMappings.Remove(tag);
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public abstract bool TryGetValue(string key, out object value);
    }
}