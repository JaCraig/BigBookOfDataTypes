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
using System.ComponentModel;
using System.Linq;

namespace BigBook
{
    /// <summary>
    /// Permutation extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PermutationExtensions
    {
        /// <summary>
        /// Finds all permutations of the items within the list
        /// </summary>
        /// <typeparam name="T">Object type in the list</typeparam>
        /// <param name="input">Input list</param>
        /// <returns>The list of permutations</returns>
        public static ListMapping<int, T> Permute<T>(this IEnumerable<T> input)
        {
            if (input == null)
                return new ListMapping<int, T>();
            var Current = new List<T>();
            Current.AddRange(input);
            var ReturnValue = new ListMapping<int, T>();
            var Max = (input.Count() - 1).Factorial();
            int CurrentValue = 0;
            for (int x = 0; x < input.Count(); ++x)
            {
                int z = 0;
                while (z < Max)
                {
                    int y = input.Count() - 1;
                    while (y > 1)
                    {
                        T TempHolder = Current[y - 1];
                        Current[y - 1] = Current[y];
                        Current[y] = TempHolder;
                        --y;
                        foreach (T Item in Current)
                            ReturnValue.Add(CurrentValue, Item);
                        ++z;
                        ++CurrentValue;
                        if (z == Max)
                            break;
                    }
                }
                if (x + 1 != input.Count())
                {
                    Current.Clear();
                    Current.AddRange(input);
                    T TempHolder2 = Current[0];
                    Current[0] = Current[x + 1];
                    Current[x + 1] = TempHolder2;
                }
            }
            return ReturnValue;
        }
    }
}