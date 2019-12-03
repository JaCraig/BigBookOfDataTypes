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
    /// Used to count the number of times something is added to the list
    /// </summary>
    /// <typeparam name="T">Type of data within the bag</typeparam>
    public class Bag<T> : ICollection<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Bag()
        {
            Items = new ConcurrentDictionary<T, int>();
        }

        /// <summary>
        /// Number of items in the bag
        /// </summary>
        public virtual int Count => Items.Count;

        /// <summary>
        /// Is this read only?
        /// </summary>
        public virtual bool IsReadOnly => false;

        /// <summary>
        /// Actual internal container
        /// </summary>
        protected ConcurrentDictionary<T, int> Items { get; }

        /// <summary>
        /// Gets a specified item
        /// </summary>
        /// <param name="index">Item to get</param>
        /// <returns>The number of this item in the bag</returns>
        public virtual int this[T index]
        {
            get => Items.GetValue(index);
            set => Items.SetValue(index, value);
        }

        /// <summary>
        /// Adds an item to the bag
        /// </summary>
        /// <param name="item">Item to add</param>
        public virtual void Add(T item) => Items.SetValue(item, Items.GetValue(item, 0) + 1);

        /// <summary>
        /// Clears the bag
        /// </summary>
        public virtual void Clear() => Items.Clear();

        /// <summary>
        /// Determines if the bag contains an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public virtual bool Contains(T item) => Items.ContainsKey(item);

        /// <summary>
        /// Copies the bag to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public virtual void CopyTo(T[] array, int arrayIndex) => Array.Copy(Items.ToList().ToArray(x => x.Key), 0, array, arrayIndex, Count);

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (var Key in Items.Keys)
            {
                yield return Key;
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var Key in Items.Keys)
            {
                yield return Key;
            }
        }

        /// <summary>
        /// Removes an item from the bag
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(T item) => Items.TryRemove(item, out _);
    }
}