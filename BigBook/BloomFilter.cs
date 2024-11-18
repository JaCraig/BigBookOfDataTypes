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

namespace BigBook
{
    /// <summary>
    /// Bloom filter
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class BloomFilter<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BloomFilter{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public BloomFilter(int size)
            : this(size, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloomFilter{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="errorRate">The error rate.</param>
        public BloomFilter(int size, float errorRate)
            : this(size, errorRate, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloomFilter{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="hashFunction">The hash function.</param>
        public BloomFilter(int size, HashFunction? hashFunction)
            : this(size, GetErrorRate(size), hashFunction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloomFilter{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="errorRate">The error rate.</param>
        /// <param name="hashFunction">The hash function.</param>
        public BloomFilter(int size, float errorRate, HashFunction? hashFunction)
            : this(size, errorRate, hashFunction, GetMValue(size, errorRate), GetKValue(size, errorRate))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BloomFilter{T}"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="errorRate">The error rate.</param>
        /// <param name="hashFunction">The hash function.</param>
        /// <param name="m">The m.</param>
        /// <param name="k">The k.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException">hashFunction</exception>
        public BloomFilter(int size, float errorRate, HashFunction? hashFunction, int m, int k)
        {
            size = size > 0 ? size : 1;
            errorRate = errorRate.Clamp(1, 0);
            if (m < 1)
                throw new ArgumentOutOfRangeException(Properties.Resources.BloomFilterCapacity);

            if (hashFunction is null)
            {
                if (typeof(TObject) == typeof(string))
                {
                    Function = HashString;
                }
                else if (typeof(TObject) == typeof(int))
                {
                    Function = HashInt32;
                }
                else
                {
                    throw new ArgumentNullException(nameof(hashFunction));
                }
            }
            else
            {
                Function = hashFunction;
            }

            HashFunctionCount = k;
            InternalBitArray = new BitArray(m);
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        private HashFunction Function { get; }

        /// <summary>
        /// Gets the hash function count.
        /// </summary>
        /// <value>The hash function count.</value>
        private int HashFunctionCount { get; }

        /// <summary>
        /// Gets the internal bit array.
        /// </summary>
        /// <value>The internal bit array.</value>
        private BitArray InternalBitArray { get; }

        /// <summary>
        /// Hash function delegate
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The hashed value.</returns>
        public delegate int HashFunction(TObject input);

        /// <summary>
        /// Adds a new item to the filter. It cannot be removed.
        /// </summary>
        /// <param name="item">The item.</param>
		public void Add(TObject item)
        {
            if (item is null)
                return;
            var primaryHash = item.GetHashCode();
            var secondaryHash = Function(item);
            for (var i = 0; i < HashFunctionCount; i++)
            {
                InternalBitArray[ComputeHash(primaryHash, secondaryHash, i)] = true;
            }
        }

        /// <summary>
        /// Checks for the existance of the item in the filter for a given probability.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(TObject item)
        {
            if (item is null)
                return false;
            var primaryHash = item.GetHashCode();
            var secondaryHash = Function(item);
            for (var i = 0; i < HashFunctionCount; i++)
            {
                if (!InternalBitArray[ComputeHash(primaryHash, secondaryHash, i)])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the error rate.
        /// </summary>
        /// <param name="size">The capacity.</param>
        /// <returns>The error rate.</returns>
        private static float GetErrorRate(int size)
        {
            var c = (float)(1.0 / size);
            return c != 0 ? c : (float)Math.Pow(0.6185, int.MaxValue / size);
        }

        /// <summary>
        /// Gets the K value.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="errorRate">The error rate.</param>
        /// <returns>The K value.</returns>
        private static int GetKValue(int size, float errorRate)
        {
            return (int)Math.Round(Math.Log(2.0) * GetMValue(size, errorRate) / size);
        }

        /// <summary>
        /// Gets the M value.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="errorRate">The error rate.</param>
        /// <returns>The M value</returns>
        private static int GetMValue(int size, float errorRate)
        {
            return (int)Math.Ceiling(size * Math.Log(errorRate, 1.0 / Math.Pow(2, Math.Log(2.0))));
        }

        /// <summary>
        /// Hashes the int32.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The hash of the int</returns>
        private static int HashInt32(TObject input)
        {
            uint? x = input as uint?;
            if (x is null)
                return 0;
            unchecked
            {
                x = ~x + (x << 15);
                x ^= x >> 12;
                x += x << 2;
                x ^= x >> 4;
                x *= 2057;
                x ^= x >> 16;
                return (int)x;
            }
        }

        /// <summary>
        /// Hashes the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The hash of the string</returns>
        private static int HashString(TObject input)
        {
            if (!(input is string s))
                return 0;
            var hash = 0;

            for (var i = 0; i < s.Length; i++)
            {
                hash += s[i];
                hash += hash << 10;
                hash ^= hash >> 6;
            }

            hash += hash << 3;
            hash ^= hash >> 11;
            hash += hash << 15;
            return hash;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="primaryHash">The primary hash.</param>
        /// <param name="secondaryHash">The secondary hash.</param>
        /// <param name="i">The i.</param>
        /// <returns>The final hash value</returns>
        private int ComputeHash(int primaryHash, int secondaryHash, int i)
        {
            var resultingHash = (primaryHash + (i * secondaryHash)) % InternalBitArray.Count;
            return Math.Abs(resultingHash);
        }
    }
}