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
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Helper class that implements a priority queue
    /// </summary>
    /// <typeparam name="T">The type of the values placed in the queue</typeparam>
    public class PriorityQueue<T> : IDictionary<int, ICollection<T>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue()
        {
            HighestKey = int.MinValue;
            Items = new Dictionary<int, ICollection<T>>();
        }

        /// <summary>
        /// The number of items in the listing
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Not read only
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// The list of keys within the mapping
        /// </summary>
        public ICollection<int> Keys => Items.Keys;

        /// <summary>
        /// List that contains the list of values
        /// </summary>
        public ICollection<ICollection<T>> Values => Keys.Select(Key => this[Key]).ToList();

        /// <summary>
        /// Highest value key
        /// </summary>
        protected int HighestKey { get; set; }

        /// <summary>
        /// Container holding the data
        /// </summary>
        protected IDictionary<int, ICollection<T>> Items { get; }

        /// <summary>
        /// Gets a list of values associated with a key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>The list of values</returns>
        public ICollection<T> this[int key]
        {
            get => Items.GetValue(key, new List<T>());
            set => Items.SetValue(key, value);
        }

        /// <summary>
        /// Adds an item to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The value to add</param>
        public void Add(int key, T value)
        {
            UpdateHighestKey(key);
            Items.SetValue(key, Items.GetValue(key, new List<T>()).Add(new T[] { value }));
        }

        /// <summary>
        /// Adds a key value pair
        /// </summary>
        /// <param name="item">Key value pair to add</param>
        public void Add(KeyValuePair<int, ICollection<T>> item)
        {
            UpdateHighestKey(item.Key);
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds a list of items to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The values to add</param>
        public void Add(int key, ICollection<T> value)
        {
            UpdateHighestKey(key);
            Items.SetValue(key, Items.GetValue(key, new List<T>()).Add(value));
        }

        /// <summary>
        /// Clears all items from the listing
        /// </summary>
        public void Clear()
        {
            HighestKey = int.MinValue;
            Items.Clear();
        }

        /// <summary>
        /// Does this contain the key value pairs?
        /// </summary>
        /// <param name="item">Key value pair to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(KeyValuePair<int, ICollection<T>> item)
        {
            return ContainsKey(item.Key) && Contains(item.Key, item.Value);
        }

        /// <summary>
        /// Does the list mapping contain the key value pairs?
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="values">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(int key, ICollection<T> values)
        {
            if (!ContainsKey(key) || values is null)
            {
                return false;
            }

            foreach (var Value in values)
            {
                if (!Contains(key, Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Does the list mapping contain the key value pair?
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(int key, T value)
        {
            return ContainsKey(key) && this[key].Contains(value);
        }

        /// <summary>
        /// Determines if a key exists
        /// </summary>
        /// <param name="key">Key to check on</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsKey(int key) => Items.ContainsKey(key);

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">array index</param>
        public void CopyTo(KeyValuePair<int, ICollection<T>>[] array, int arrayIndex) => throw new NotImplementedException();

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        public IEnumerator<KeyValuePair<int, ICollection<T>>> GetEnumerator()
        {
            foreach (var Key in Keys)
            {
                yield return new KeyValuePair<int, ICollection<T>>(Key, this[Key]);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var Key in Keys)
            {
                yield return this[Key];
            }
        }

        /// <summary>
        /// Peek at the next thing in the queue
        /// </summary>
        /// <returns>The next item in queue or default(T) if it is empty</returns>
        [return: MaybeNull]
        public T Peek()
        {
            return Items.ContainsKey(HighestKey) ? Items[HighestKey].FirstOrDefault() : (default);
        }

        /// <summary>
        /// Removes an item from the queue and returns it
        /// </summary>
        /// <returns>The next item in the queue</returns>
        [return: MaybeNull]
        public T Pop()
        {
            if (!Items.ContainsKey(HighestKey) || Items[HighestKey].Count == 0)
                return default!;
            var ReturnValue = Items[HighestKey].FirstOrDefault();
            Remove(HighestKey, ReturnValue);
            if (!ContainsKey(HighestKey))
            {
                HighestKey = int.MinValue;
                foreach (var Key in Items.Keys)
                {
                    if (Key > HighestKey)
                    {
                        HighestKey = Key;
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Remove a list of items associated with a key
        /// </summary>
        /// <param name="key">Key to use</param>
        /// <returns>True if the key is found, false otherwise</returns>
        public bool Remove(int key) => Items.Remove(key);

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="item">items to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<int, ICollection<T>> item)
        {
            if (!Contains(item))
            {
                return false;
            }

            foreach (var Value in item.Value)
            {
                if (!Remove(item.Key, Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <param name="Value">Value to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(int key, T Value)
        {
            if (!Contains(key, Value))
            {
                return false;
            }

            Items[key].Remove(Value);
            if (this[key].Count == 0)
            {
                Remove(key);
            }

            return true;
        }

        /// <summary>
        /// Tries to get the value associated with the key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The values getting</param>
        /// <returns>True if it was able to get the value, false otherwise</returns>
        public bool TryGetValue(int key, out ICollection<T> value) => Items.TryGetValue(key, out value);

        /// <summary>
        /// Updates the highest key.
        /// </summary>
        /// <param name="key">The key.</param>
        private void UpdateHighestKey(int key)
        {
            if (key > HighestKey)
            {
                HighestKey = key;
            }
        }
    }
}