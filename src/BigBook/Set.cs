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

using System.Collections.Generic;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Class to be used for sets of data
    /// </summary>
    /// <typeparam name="T">Type that the set holds</typeparam>
    public class Set<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Set{T}"/> class.
        /// </summary>
        public Set()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialSize">Initial size</param>
        public Set(int initialSize)
            : base(initialSize < 0 ? 0 : initialSize)
        {
        }

        /// <summary>
        /// Gets the intersection of set 1 and set 2
        /// </summary>
        /// <param name="set1">Set 1</param>
        /// <param name="set2">Set 2</param>
        /// <returns>The intersection of the two sets</returns>
        public static Set<T> GetIntersection(Set<T> set1, Set<T> set2)
        {
            if (set1 == null || set2 == null || !set1.Intersect(set2))
            {
                return null;
            }

            var ReturnValue = new Set<T>();
            for (var x = 0; x < set1.Count; ++x)
            {
                if (set2.Contains(set1[x]))
                {
                    ReturnValue.Add(set1[x]);
                }
            }

            for (var x = 0; x < set2.Count; ++x)
            {
                if (set1.Contains(set2[x]))
                {
                    ReturnValue.Add(set2[x]);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Removes items from set 2 from set 1
        /// </summary>
        /// <param name="set1">Set 1</param>
        /// <param name="set2">Set 2</param>
        /// <returns>The resulting set</returns>
        public static Set<T> operator -(Set<T> set1, Set<T> set2)
        {
            set1 = set1 ?? new Set<T>();
            set2 = set2 ?? new Set<T>();
            var ReturnValue = new Set<T>();
            for (var x = 0; x < set1.Count; ++x)
            {
                if (!set2.Contains(set1[x]))
                {
                    ReturnValue.Add(set1[x]);
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are not equivalent
        /// </summary>
        /// <param name="set1">Set 1</param>
        /// <param name="set2">Set 2</param>
        /// <returns>False if they are, true otherwise</returns>
        public static bool operator !=(Set<T> set1, Set<T> set2)
        {
            return !(set1 == set2);
        }

        /// <summary>
        /// Adds two sets together
        /// </summary>
        /// <param name="set1">Set 1</param>
        /// <param name="set2">Set 2</param>
        /// <returns>The joined sets</returns>
        public static Set<T> operator +(Set<T> set1, Set<T> set2)
        {
            set1 = set1 ?? new Set<T>();
            set2 = set2 ?? new Set<T>();
            var ReturnValue = new Set<T>();
            for (var x = 0; x < set1.Count; ++x)
            {
                ReturnValue.Add(set1[x]);
            }

            for (var x = 0; x < set2.Count; ++x)
            {
                ReturnValue.Add(set2[x]);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="set1">Set 1</param>
        /// <param name="set2">Set 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Set<T> set1, Set<T> set2)
        {
            if (((object)set1) == null && ((object)set2) == null)
            {
                return true;
            }

            if (((object)set1) == null || ((object)set2) == null)
            {
                return false;
            }

            return set1.Contains(set2) && set2.Contains(set1);
        }

        /// <summary>
        /// Used to tell if this set contains the other
        /// </summary>
        /// <param name="set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool Contains(Set<T> set)
        {
            if (set == null)
            {
                return false;
            }

            return set.IsSubset(this);
        }

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj) => this == (obj as Set<T>);

        /// <summary>
        /// Returns the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Determines if the sets intersect
        /// </summary>
        /// <param name="set">Set to check against</param>
        /// <returns>True if they do, false otherwise</returns>
        public bool Intersect(Set<T> set)
        {
            if (set == null)
            {
                return false;
            }

            for (var x = 0; x < Count; ++x)
            {
                if (set.Contains(this[x]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Used to tell if this is a subset of the other
        /// </summary>
        /// <param name="set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool IsSubset(Set<T> set)
        {
            if (set == null || Count > set.Count)
            {
                return false;
            }

            for (var x = 0; x < Count; ++x)
            {
                if (!set.Contains(this[x]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the set as a string
        /// </summary>
        /// <returns>The set as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.Append("{ ");
            var Splitter = "";
            for (var x = 0; x < Count; ++x)
            {
                Builder.Append(Splitter)
                       .AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}", this[x]);
                Splitter = ",  ";
            }
            Builder.Append(" }");
            return Builder.ToString();
        }
    }
}