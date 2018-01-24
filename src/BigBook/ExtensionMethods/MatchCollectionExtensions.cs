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
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BigBook
{
    /// <summary>
    /// MatchCollection extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MatchCollectionExtensions
    {
        /// <summary>
        /// Gets a list of items that satisfy the predicate from the collection
        /// </summary>
        /// <param name="collection">Collection to search through</param>
        /// <param name="predicate">Predicate that the items must satisfy</param>
        /// <returns>The matches that satisfy the predicate</returns>
        public static IEnumerable<Match> Where(this MatchCollection collection, Predicate<Match> predicate)
        {
            if (predicate == null || collection == null)
                yield break;
            for (int x = 0, collectionCount = collection.Count; x < collectionCount; x++)
            {
                Match Item = collection[x];
                if (predicate(Item))
                    yield return Item;
            }
        }
    }
}