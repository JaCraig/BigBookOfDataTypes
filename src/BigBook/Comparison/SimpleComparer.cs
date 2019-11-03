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
    /// Simple IComparable class
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class SimpleComparer<T> : IComparer<T> where T : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleComparer{T}"/> class.
        /// </summary>
        /// <param name="comparisonFunction">The comparison function.</param>
        public SimpleComparer(Func<T, T, int> comparisonFunction)
        {
            ComparisonFunction = comparisonFunction;
        }

        /// <summary>
        /// Gets or sets the comparison function.
        /// </summary>
        /// <value>The comparison function.</value>
        protected Func<T, T, int> ComparisonFunction { get; set; }

        /// <summary>
        /// Compares the two objects
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>0 if they're equal, any other value they are not</returns>
        public int Compare(T x, T y) => ComparisonFunction(x, y);
    }
}