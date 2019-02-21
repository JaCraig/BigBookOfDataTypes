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
using System.Collections;
using System.Collections.Generic;

namespace BigBook.Comparison
{
    /// <summary>
    /// Generic equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Determines if the two items are equal
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public bool Equals(T x, T y)
        {
            var TypeInfo = typeof(T);
            if (!TypeInfo.IsValueType
                || (TypeInfo.IsGenericType
                && TypeInfo.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                if (object.Equals(x, default(T)))
                {
                    return object.Equals(y, default(T));
                }

                if (object.Equals(y, default(T)))
                {
                    return false;
                }
            }
            if (x.GetType() != y.GetType())
            {
                return false;
            }

            var IEnumerabley = y as IEnumerable;
            if (x is IEnumerable IEnumerablex && IEnumerabley != null)
            {
                var Comparer = new GenericEqualityComparer<object>();
                var XEnumerator = IEnumerablex.GetEnumerator();
                var YEnumerator = IEnumerabley.GetEnumerator();
                while (true)
                {
                    bool XFinished = !XEnumerator.MoveNext();
                    bool YFinished = !YEnumerator.MoveNext();
                    if (XFinished || YFinished)
                    {
                        return XFinished && YFinished;
                    }

                    if (!Comparer.Equals(XEnumerator.Current, YEnumerator.Current))
                    {
                        return false;
                    }
                }
            }
            if (x is IEqualityComparer<T> TempEquality)
            {
                return TempEquality.Equals(y);
            }

            if (x is IComparable<T> TempComparable)
            {
                return TempComparable.CompareTo(y) == 0;
            }

            if (x is IComparable TempComparable2)
            {
                return TempComparable2.CompareTo(y) == 0;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <param name="obj">Object to get the hash code of</param>
        /// <returns>The object's hash code</returns>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}