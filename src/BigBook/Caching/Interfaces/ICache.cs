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

namespace BigBook.Caching.Interfaces
{
    /// <summary>
    /// Cache interface
    /// </summary>
    public interface ICache : IDictionary<string, object>, IDisposable
    {
        /// <summary>
        /// Cache name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The tags used thus far
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// Adds a value/key combination and assigns tags to it
        /// </summary>
        /// <param name="key">Key to add</param>
        /// <param name="tags">Tags to associate with the key/value pair</param>
        /// <param name="value">Value to add</param>
        void Add(string key, object value, IEnumerable<string> tags);

        /// <summary>
        /// Gets the objects associated with a specific tag
        /// </summary>
        /// <param name="tag">Tag to use</param>
        /// <returns>The objects associated with the tag</returns>
        IEnumerable<object> GetByTag(string tag);

        /// <summary>
        /// Removes all items associated with the tag specified
        /// </summary>
        /// <param name="tag">Tag to remove</param>
        void RemoveByTag(string tag);
    }
}