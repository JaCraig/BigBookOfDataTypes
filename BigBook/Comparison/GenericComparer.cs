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
    /// Generic IComparable class
    /// </summary>
    /// <typeparam name="TData">Data type</typeparam>
    public class GenericComparer<TData> : IComparer<TData> where TData : IComparable
    {
        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>The comparer.</value>
        public static GenericComparer<TData> Comparer { get; } = new GenericComparer<TData>();

        /// <summary>
        /// Compares the two objects
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>0 if they're equal, any other value they are not</returns>
        public int Compare(TData x, TData y)
        {
            var TypeInfo = typeof(TData);
            if (!TypeInfo.IsValueType
                || (TypeInfo.IsGenericType
                && TypeInfo.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                if (Equals(x, default(TData)!))
                {
                    return Equals(y, default(TData)!) ? 0 : -1;
                }

                if (Equals(y, default(TData)!))
                {
                    return -1;
                }
            }
            if (x.GetType() != y.GetType())
            {
                return -1;
            }

            if (x is IComparable<TData> TempComparable)
            {
                return TempComparable.CompareTo(y);
            }

            return x.CompareTo(y);
        }
    }
}