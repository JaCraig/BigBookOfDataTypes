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

using BigBook.Caching.Default;
using BigBook.Caching.Interfaces;
using BigBook.Patterns.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.Caching
{
    /// <summary>
    /// Caching manager class
    /// </summary>
    /// <seealso cref="BigBook.Patterns.BaseClasses.SafeDisposableBaseClass"/>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="caches">The caches.</param>
        public Manager(IEnumerable<ICache> caches)
        {
            ExtensionMethods.CacheExtensions.CacheManager = this;
            caches ??= Array.Empty<ICache>();
            Caches = caches.Where(x => x.GetType().Assembly != typeof(Manager).Assembly).ToDictionary(x => x.Name);
            if (!Caches.ContainsKey("Default"))
            {
                Caches.Add("Default", new Cache());
            }
        }

        /// <summary>
        /// To string
        /// </summary>
        private string? _ToString;

        /// <summary>
        /// Caches
        /// </summary>
        protected Dictionary<string, ICache> Caches { get; }

        /// <summary>
        /// Gets the specified cache
        /// </summary>
        /// <param name="name">Name of the cache (defaults to Default)</param>
        /// <returns>
        /// Returns the ICache specified if it exists, otherwise creates a default cache and
        /// associates it with the name
        /// </returns>
        public ICache Cache(string name = "Default")
        {
            if (Caches.TryGetValue(name, out var ReturnValue))
                return ReturnValue;
            _ToString = null;
            ReturnValue = new Cache();
            Caches.Add(name, ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Outputs the manager as a string
        /// </summary>
        /// <returns>String version of the manager</returns>
        public override string ToString()
        {
            return _ToString ?? (_ToString = $"Caches: {Caches.ToString(x => x.Key)}\r\n");
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (Caches is null)
                return;
            Caches.ForEach(x => x.Value.Dispose());
            Caches.Clear();
        }
    }
}