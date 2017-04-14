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
using System.Reflection;

namespace BigBook.Comparison
{
    /// <summary>
    /// Generic IComparable class
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericComparer<T> : IComparer<T> where T : IComparable
    {
        /// <summary>
        /// Compares the two objects
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>0 if they're equal, any other value they are not</returns>
        public int Compare(T x, T y)
        {
            var TypeInfo = typeof(T).GetTypeInfo();
            if (!TypeInfo.IsValueType
                || (TypeInfo.IsGenericType
                && TypeInfo.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                if (Equals(x, default(T)))
                    return Equals(y, default(T)) ? 0 : -1;
                if (Equals(y, default(T)))
                    return -1;
            }
            if (x.GetType() != y.GetType())
                return -1;
            if (x is IComparable<T> TempComparable)
                return TempComparable.CompareTo(y);
            return x.CompareTo(y);
        }
    }
}