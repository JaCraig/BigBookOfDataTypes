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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Dictionary that matches multiple keys to each value
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public class TagDictionary<TKey, TValue> : IDictionary<TKey, IEnumerable<TValue>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TagDictionary()
        {
            Items = new ConcurrentBag<TaggedItem>();
            KeyList = new List<TKey>();
        }

        /// <summary>
        /// Number of items in the dictionary
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Always false
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Gets the keys found in the dictionary
        /// </summary>
        public ICollection<TKey> Keys => KeyList;

        /// <summary>
        /// Gets the values found in the dictionary
        /// </summary>
        public ICollection<IEnumerable<TValue>> Values => new IEnumerable<TValue>[] { Items.ToArray(x => x.Value) };

        /// <summary>
        /// Items in the dictionary
        /// </summary>
        private ConcurrentBag<TaggedItem> Items { get; set; }

        /// <summary>
        /// List of keys that have been entered
        /// </summary>
        private List<TKey> KeyList { get; }

        /// <summary>
        /// Gets the values based on a key
        /// </summary>
        /// <param name="key">Key to get the values of</param>
        /// <returns>The values associated with the key</returns>
        public IEnumerable<TValue> this[TKey key]
        {
            get => Items.Where(x => x.Keys.Contains(key)).ToArray(x => x.Value);
            set => Add(key, value);
        }

        /// <summary>
        /// Adds a list of values to the key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Values to add</param>
        public void Add(TKey key, IEnumerable<TValue> value)
        {
            value.ToArray(x => new TaggedItem(key, x)).ForEach(x => Items.Add(x));
            KeyList.AddIfUnique(key);
        }

        /// <summary>
        /// Adds a value to the dicionary
        /// </summary>
        /// <param name="value">Value to add</param>
        /// <param name="keys">Keys to associate the value with</param>
        public void Add(TValue value, params TKey[] keys)
        {
            keys ??= Array.Empty<TKey>();
            Items.Add(new TaggedItem(keys, value));
            keys.ForEach(x => KeyList.AddIfUnique(x));
        }

        /// <summary>
        /// Adds an item to the dictionary
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<TKey, IEnumerable<TValue>> item) => Add(item.Key, item.Value);

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear() => Items.Clear();

        /// <summary>
        /// Determines if the dictionary contains the key/value pair
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool Contains(KeyValuePair<TKey, IEnumerable<TValue>> item) => ContainsKey(item.Key);

        /// <summary>
        /// Determines if a key is in the dictionary
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsKey(TKey key) => KeyList.Contains(key);

        /// <summary>
        /// Copies itself to an array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(KeyValuePair<TKey, IEnumerable<TValue>>[] array, int arrayIndex)
        {
            if (array is null)
                return;
            for (var x = 0; x < Keys.Count; ++x)
            {
                array[arrayIndex + x] = new KeyValuePair<TKey, IEnumerable<TValue>>(Keys.ElementAt(x), this[Keys.ElementAt(x)]);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            foreach (var TempKey in Keys)
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(TempKey, this[TempKey]);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var TempKey in Keys)
            {
                yield return this[TempKey];
            }
        }

        /// <summary>
        /// Removes all items that are associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns true if the key was found, false otherwise</returns>
        public bool Remove(TKey key)
        {
            var ReturnValue = ContainsKey(key);
            Items = new ConcurrentBag<TaggedItem>(Items.ToArray(x => x).Where(x => !x.Keys.Contains(key)));
            KeyList.Remove(key);
            return ReturnValue;
        }

        /// <summary>
        /// Removes a specific key/value pair
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<TKey, IEnumerable<TValue>> item) => Remove(item.Key);

        /// <summary>
        /// Attempts to get the values associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Values associated with a key</param>
        /// <returns>True if something is returned, false otherwise</returns>
        public bool TryGetValue(TKey key, out IEnumerable<TValue> value)
        {
            value = Array.Empty<TValue>();
            try
            {
                value = this[key];
            }
            catch { }
            return value.Any();
        }

        /// <summary>
        /// Holds information about each value
        /// </summary>
        private class TaggedItem
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="keys">Keys</param>
            /// <param name="value">Value</param>
            public TaggedItem(TKey[] keys, TValue value)
            {
                Keys = keys;
                Value = value;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="key">Key</param>
            /// <param name="value">Value</param>
            public TaggedItem(TKey key, TValue value)
                : this(new TKey[] { key }, value)
            {
            }

            /// <summary>
            /// The list of keys associated with the value
            /// </summary>
            public TKey[] Keys { get; set; }

            /// <summary>
            /// Value
            /// </summary>
            public TValue Value { get; set; }
        }
    }
}