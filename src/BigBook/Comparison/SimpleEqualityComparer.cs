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

namespace BigBook.Comparison
{
    /// <summary>
    /// Simple equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class SimpleEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="comparisonFunction">The comparison function.</param>
        /// <param name="hashFunction">The hash function.</param>
        public SimpleEqualityComparer(Func<T, T, bool> comparisonFunction, Func<T, int> hashFunction)
        {
            ComparisonFunction = comparisonFunction;
            HashFunction = hashFunction;
        }

        /// <summary>
        /// Gets or sets the comparison function.
        /// </summary>
        /// <value>The comparison function.</value>
        protected Func<T, T, bool> ComparisonFunction { get; set; }

        /// <summary>
        /// Gets or sets the hash function.
        /// </summary>
        /// <value>The hash function.</value>
        protected Func<T, int> HashFunction { get; set; }

        /// <summary>
        /// Determines if the two items are equal
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public bool Equals(T x, T y)
        {
            return ComparisonFunction(x, y);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <param name="obj">Object to get the hash code of</param>
        /// <returns>The object's hash code</returns>
        public int GetHashCode(T obj)
        {
            return HashFunction(obj);
        }
    }
}