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
using System.ComponentModel;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Array extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ArrayExtensions
    {
        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <param name="array">Array to clear</param>
        /// <returns>The final array</returns>
        public static Array? Clear(this Array array)
        {
            if (array is null)
                return null;

            Array.Clear(array, 0, array.Length);
            return array;
        }

        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <typeparam name="TArrayType">Array type</typeparam>
        /// <param name="array">Array to clear</param>
        /// <returns>The final array</returns>
        public static TArrayType[]? Clear<TArrayType>(this TArrayType[] array)
        {
            if (array is null)
            {
                return null;
            }

            Array.Clear(array, 0, array.Length);
            return array;
        }

        /// <summary>
        /// Combines two arrays and returns a new array containing both values
        /// </summary>
        /// <typeparam name="TArrayType">Type of the data in the array</typeparam>
        /// <param name="array1">Array 1</param>
        /// <param name="additions">Arrays to concat onto the first item</param>
        /// <returns>A new array containing both arrays' values</returns>
        public static TArrayType[] Concat<TArrayType>(this TArrayType[] array1, params TArrayType[][] additions)
        {
            array1 ??= Array.Empty<TArrayType>();
            additions ??= Array.Empty<TArrayType[]>();
            var finalAdditions = additions.Where(x => !(x is null));
            var Result = new TArrayType[array1.Length + finalAdditions.Sum(x => x.Length)];
            var Offset = array1.Length;
            Array.Copy(array1, 0, Result, 0, array1.Length);
            foreach (var item in finalAdditions)
            {
                Array.Copy(item, 0, Result, Offset, item.Length);
                Offset += item.Length;
            }
            return Result;
        }
    }
}