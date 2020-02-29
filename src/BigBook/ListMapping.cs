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
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Maps a key to a list of data
    /// </summary>
    /// <typeparam name="T1">Key value</typeparam>
    /// <typeparam name="T2">Type that the list should contain</typeparam>
    public class ListMapping<T1, T2> : IDictionary<T1, IEnumerable<T2>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ListMapping()
        {
            Items = new ConcurrentDictionary<T1, ConcurrentBag<T2>>();
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
        public ICollection<T1> Keys => Items.Keys;

        /// <summary>
        /// List that contains the list of values
        /// </summary>
        public ICollection<IEnumerable<T2>> Values => this.Select(x => x.Value).ToList();

        /// <summary>
        /// Container holding the data
        /// </summary>
        protected ConcurrentDictionary<T1, ConcurrentBag<T2>> Items { get; }

        /// <summary>
        /// Gets a list of values associated with a key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>The list of values</returns>
        public IEnumerable<T2> this[T1 key]
        {
            get => Items.GetValue(key, new ConcurrentBag<T2>());
            set => Items.SetValue(key, new ConcurrentBag<T2>(value));
        }

        /// <summary>
        /// Adds an item to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The value to add</param>
        public void Add(T1 key, T2 value)
        {
            Items.AddOrUpdate(key,
                              _ => new ConcurrentBag<T2>(),
                              (_, y) => y)
                 .Add(value);
        }

        /// <summary>
        /// Adds a key value pair
        /// </summary>
        /// <param name="item">Key value pair to add</param>
        public void Add(KeyValuePair<T1, IEnumerable<T2>> item) => Add(item.Key, item.Value);

        /// <summary>
        /// Adds a list of items to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The values to add</param>
        public void Add(T1 key, IEnumerable<T2> value)
        {
            Items.AddOrUpdate(key,
                              _ => new ConcurrentBag<T2>(),
                              (_, y) => y)
                 .Add(value);
        }

        /// <summary>
        /// Clears all items from the listing
        /// </summary>
        public void Clear() => Items.Clear();

        /// <summary>
        /// Does this contain the key value pairs?
        /// </summary>
        /// <param name="item">Key value pair to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(KeyValuePair<T1, IEnumerable<T2>> item) => ContainsKey(item.Key) && Contains(item.Key, item.Value);

        /// <summary>
        /// Does the list mapping contain the key value pairs?
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="values">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(T1 key, IEnumerable<T2> values) => ContainsKey(key) && values.All(x => Contains(key, x));

        /// <summary>
        /// Does the list mapping contain the key value pair?
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(T1 key, T2 value) => ContainsKey(key) && Items[key].Contains(value);

        /// <summary>
        /// Determines if a key exists
        /// </summary>
        /// <param name="key">Key to check on</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsKey(T1 key) => Items.ContainsKey(key);

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">array index</param>
        public void CopyTo(KeyValuePair<T1, IEnumerable<T2>>[] array, int arrayIndex) => throw new NotImplementedException();

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        public IEnumerator<KeyValuePair<T1, IEnumerable<T2>>> GetEnumerator()
        {
            foreach (var Key in Keys)
            {
                yield return new KeyValuePair<T1, IEnumerable<T2>>(Key, this[Key]);
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
        /// Remove a list of items associated with a key
        /// </summary>
        /// <param name="key">Key to use</param>
        /// <returns>True if the key is found, false otherwise</returns>
        public bool Remove(T1 key)
        {
            _ = new ConcurrentBag<T2>();
            return Items.TryRemove(key, out _);
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="item">items to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<T1, IEnumerable<T2>> item)
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
        /// <param name="value">Value to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(T1 key, T2 value)
        {
            if (!Contains(key, value))
            {
                return false;
            }

            var TempValue = Items[key].ToList(z => z);
            TempValue.Remove(value);
            Items.AddOrUpdate(key,
                new ConcurrentBag<T2>(TempValue),
                (_, __) => new ConcurrentBag<T2>(TempValue));
            if (!this[key].Any())
            {
                Remove(key);
            }

            return true;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (var Key in Keys)
            {
                Builder.AppendLineFormat("{0}:{{{1}}}", Key?.ToString() ?? "", Items[Key].ToString(x => x?.ToString() ?? ""));
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Tries to get the value associated with the key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The values getting</param>
        /// <returns>True if it was able to get the value, false otherwise</returns>
        public bool TryGetValue(T1 key, out IEnumerable<T2> value)
        {
            if (Items.TryGetValue(key, out var TempValue))
            {
                value = TempValue.ToArray(x => x);
                return true;
            }
            value = Array.Empty<T2>();
            return false;
        }
    }
}