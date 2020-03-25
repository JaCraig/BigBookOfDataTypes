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

namespace BigBook.DynamoUtils
{
    /// <summary>
    /// Dynamo class
    /// </summary>
    internal class DynamoClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoClass"/> class.
        /// </summary>
        public DynamoClass()
        {
            HashCode = EmptyHashCode;
            Keys = Array.Empty<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoClass"/> class.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="hashCode">The hash code.</param>
        public DynamoClass(string[] keys, int hashCode)
        {
            HashCode = hashCode;
            Keys = keys;
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public string[] Keys { get; }

        /// <summary>
        /// Gets or sets the hash code.
        /// </summary>
        /// <value>The hash code.</value>
        private int HashCode { get; }

        /// <summary>
        /// Gets or sets the sub classes.
        /// </summary>
        /// <value>The sub classes.</value>
        private Dictionary<int, List<WeakReference>>? SubClasses { get; set; }

        /// <summary>
        /// The empty object.
        /// </summary>
        public static DynamoClass Empty = new DynamoClass();

        /// <summary>
        /// The empty hash code
        /// </summary>
        private const int EmptyHashCode = 6551;

        /// <summary>
        /// Adds a key and finds or creates a DynamoClass.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The DynamoClass specified.</returns>
        public DynamoClass AddKey(string key)
        {
            int TempHashCode = HashCode ^ key.GetHashCode(StringComparison.OrdinalIgnoreCase);
            lock (this)
            {
                var weakReferences = GetList(TempHashCode);
                for (int x = 0; x < weakReferences.Count; ++x)
                {
                    if (!(weakReferences[x].Target is DynamoClass @class))
                    {
                        weakReferences.RemoveAt(x);
                        --x;
                        continue;
                    }
                    if (string.Equals(@class.Keys[^1], key, StringComparison.OrdinalIgnoreCase))
                        return @class;
                }

                var TempKeys = new string[Keys.Length + 1];
                Array.Copy(Keys, TempKeys, Keys.Length);
                TempKeys[^1] = key;
                var NewClass = new DynamoClass(TempKeys, TempHashCode);
                weakReferences.Add(new WeakReference(NewClass));
                return NewClass;
            }
        }

        /// <summary>
        /// Gets the index for the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The index for the key.</returns>
        public int GetIndex(string key)
        {
            for (int x = 0; x < Keys.Length; ++x)
            {
                if (string.Equals(Keys[x], key, StringComparison.OrdinalIgnoreCase))
                    return x;
            }
            return -1;
        }

        /// <summary>
        /// Gets the list of WeakReferences based on the hash code.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <returns>The list requested.</returns>
        private List<WeakReference> GetList(int hashCode)
        {
            SubClasses ??= new Dictionary<int, List<WeakReference>>();
            if (!SubClasses.TryGetValue(hashCode, out var ReturnValue))
            {
                SubClasses[hashCode] = ReturnValue = new List<WeakReference>();
            }
            return ReturnValue;
        }
    }
}