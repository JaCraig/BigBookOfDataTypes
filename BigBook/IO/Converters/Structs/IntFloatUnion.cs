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
using System.Runtime.InteropServices;

namespace BigBook.IO.Converters.Structs
{
    /// <summary>
    /// Int/float union struct.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IntFloatUnion : IEquatable<IntFloatUnion>
    {
        /// <summary>
        /// The integer value
        /// </summary>
        [FieldOffset(0)]
        public readonly int IntegerValue;

        /// <summary>
        /// The float value
        /// </summary>
        [FieldOffset(0)]
        public readonly float FloatValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntFloatUnion"/> struct.
        /// </summary>
        /// <param name="integerValue">The integer value.</param>
        public IntFloatUnion(int integerValue)
        {
            FloatValue = 0;
            IntegerValue = integerValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntFloatUnion"/> struct.
        /// </summary>
        /// <param name="floatValue">The float value.</param>
        public IntFloatUnion(float floatValue)
        {
            IntegerValue = 0;
            FloatValue = floatValue;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is IntFloatUnion intFloatUnion && Equals(intFloatUnion);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref>
        /// parameter; otherwise, false.
        /// </returns>
        public bool Equals(IntFloatUnion other) => IntegerValue == other.IntegerValue;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode() => -1352667302 + IntegerValue.GetHashCode();

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="union1">The union1.</param>
        /// <param name="union2">The union2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(IntFloatUnion union1, IntFloatUnion union2)
        {
            return union1.Equals(union2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="union1">The union1.</param>
        /// <param name="union2">The union2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(IntFloatUnion union1, IntFloatUnion union2)
        {
            return !(union1 == union2);
        }
    }
}