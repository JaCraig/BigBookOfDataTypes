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

namespace BigBook
{
    /// <summary>
    /// Change class
    /// </summary>
    /// <seealso cref="IEquatable{Change}"/>
    public struct Change : IEquatable<Change>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalValue">Original value</param>
        /// <param name="newValue">New value</param>
        public Change(object? originalValue, object? newValue)
        {
            OriginalValue = originalValue;
            NewValue = newValue;
        }

        /// <summary>
        /// New value
        /// </summary>
        public object? NewValue { get; }

        /// <summary>
        /// Original value
        /// </summary>
        public object? OriginalValue { get; }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Change left, Change right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Change left, Change right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> and this instance are the same type and
        /// represent the same value; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Change change && Equals(change);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Change other)
        {
            return EqualityComparer<object?>.Default.Equals(NewValue, other.NewValue)
                && EqualityComparer<object?>.Default.Equals(OriginalValue, other.OriginalValue);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode() => HashCode.Combine(NewValue, OriginalValue);
    }
}