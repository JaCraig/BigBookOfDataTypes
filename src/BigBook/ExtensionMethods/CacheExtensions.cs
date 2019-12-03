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

using System.ComponentModel;

namespace BigBook.ExtensionMethods
{
    /// <summary>
    /// Extension methods relating to caching of data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CacheExtensions
    {
        /// <summary>
        /// Cacnes the object based on the key and cache specified
        /// </summary>
        /// <param name="objectToCache">Object to cache</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheName">Name of the cache to use</param>
        public static void Cache(this object objectToCache, string key, string cacheName = "Default")
        {
            if (Canister.Builder.Bootstrapper == null)
            {
                return;
            }

            Canister.Builder.Bootstrapper.Resolve<Caching.Manager>().Cache(cacheName).Add(key, objectToCache);
        }

        /// <summary>
        /// Gets the specified object from the cache
        /// </summary>
        /// <typeparam name="T">Type to convert the object to</typeparam>
        /// <param name="key">Key to the object</param>
        /// <param name="defaultValue">Default value if the key is not found</param>
        /// <param name="cacheName">Cache to get the item from</param>
        /// <returns>The object specified or the default value if it is not found</returns>
        public static T GetFromCache<T>(this string key, T defaultValue = default, string cacheName = "Default")
        {
            if (Canister.Builder.Bootstrapper == null)
            {
                return defaultValue;
            }

            var Value = Canister.Builder.Bootstrapper.Resolve<Caching.Manager>().Cache(cacheName)[key];
            return Value == null ? defaultValue : Value.To(defaultValue);
        }
    }
}