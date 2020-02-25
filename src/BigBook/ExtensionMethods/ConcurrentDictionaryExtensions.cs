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

using System.Collections.Concurrent;
using System.ComponentModel;

namespace BigBook
{
    /// <summary>
    /// ConcurrentDictionary extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// Copies the dictionary to another dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="target">The target dictionary.</param>
        /// <returns>This</returns>
        public static ConcurrentDictionary<TKey, TValue> CopyTo<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, ConcurrentDictionary<TKey, TValue> target)
        {
            dictionary ??= new ConcurrentDictionary<TKey, TValue>();
            if (target is null)
                return dictionary;
            foreach (var x in dictionary)
            {
                target.SetValue(x.Key, x.Value);
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
        public static TValue GetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary is null)
            {
                return defaultValue;
            }

            return dictionary.TryGetValue(key, out var ReturnValue) ? ReturnValue : defaultValue;
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
        public static ConcurrentDictionary<TKey, TValue> SetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary is null)
            {
                return new ConcurrentDictionary<TKey, TValue>();
            }

            dictionary.AddOrUpdate(key, value, (_, __) => value);
            return dictionary;
        }
    }
}