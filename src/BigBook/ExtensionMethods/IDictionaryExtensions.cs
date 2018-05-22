﻿/*
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// IDictionary extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Copies the dictionary to another dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="target">The target dictionary.</param>
        /// <returns>This</returns>
        public static IDictionary<TKey, TValue> CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            IDictionary<TKey, TValue> target)
        {
            if (dictionary == null)
            {
                return new Dictionary<TKey, TValue>();
            }

            if (target == null)
            {
                return dictionary;
            }

            foreach (KeyValuePair<TKey, TValue> Pair in dictionary)
            {
                target.SetValue(Pair.Key, Pair.Value);
            }
            return dictionary;
        }

        /// <summary>
        /// Gets the value from a dictionary or the default value if it isn't found
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">Dictionary to get the value from</param>
        /// <param name="key">Key to look for</param>
        /// <param name="defaultValue">Default value if the key is not found</param>
        /// <returns>
        /// The value associated with the key or the default value if the key is not found
        /// </returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            if (dictionary == null)
            {
                return defaultValue;
            }

            TValue ReturnValue = defaultValue;
            return dictionary.TryGetValue(key, out ReturnValue) ? ReturnValue : defaultValue;
        }

        /// <summary>
        /// Sets the value in a dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">Dictionary to set the value in</param>
        /// <param name="key">Key to look for</param>
        /// <param name="value">Value to add</param>
        /// <returns>The dictionary</returns>
        public static IDictionary<TKey, TValue> SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
        {
            if (dictionary == null)
            {
                return new Dictionary<TKey, TValue>();
            }

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Sorts a dictionary
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <param name="dictionary">Dictionary to sort</param>
        /// <param name="comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> Sort<T1, T2>(this IDictionary<T1, T2> dictionary, IComparer<T1> comparer = null)
            where T1 : IComparable
        {
            if (dictionary == null)
            {
                return new Dictionary<T1, T2>();
            }

            comparer = comparer ?? new GenericComparer<T1>();
            return dictionary.Sort(x => x.Key, comparer);
        }

        /// <summary>
        /// Sorts a dictionary
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <typeparam name="T3">Order by type</typeparam>
        /// <param name="dictionary">Dictionary to sort</param>
        /// <param name="orderBy">Function used to order the dictionary</param>
        /// <param name="comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> Sort<T1, T2, T3>(this IDictionary<T1, T2> dictionary,
            Func<KeyValuePair<T1, T2>, T3> orderBy,
            IComparer<T3> comparer = null)
            where T3 : IComparable
        {
            if (dictionary == null)
            {
                return new Dictionary<T1, T2>();
            }

            if (orderBy == null)
            {
                return dictionary;
            }

            comparer = comparer ?? new GenericComparer<T3>();
            return dictionary.OrderBy(orderBy, comparer).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}