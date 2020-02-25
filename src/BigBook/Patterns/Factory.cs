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

namespace BigBook.Patterns
{
    /// <summary>
    /// Factory class
    /// </summary>
    /// <typeparam name="TKey">The "message" type</typeparam>
    /// <typeparam name="TClass">The class type that you want created</typeparam>
    public class Factory<TKey, TClass>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Factory()
        {
            Constructors = new Dictionary<TKey, Func<TClass>>();
        }

        /// <summary>
        /// List of constructors/initializers
        /// </summary>
        protected Dictionary<TKey, Func<TClass>> Constructors { get; }

        /// <summary>
        /// Creates an instance associated with the key
        /// </summary>
        /// <param name="key">Registered item</param>
        /// <returns>The type returned by the initializer</returns>
        public TClass Create(TKey key) => Constructors.GetValue(key, () => default!)();

        /// <summary>
        /// Determines if a key has been registered
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Exists(TKey key) => Constructors.ContainsKey(key);

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="key">Item to register</param>
        /// <param name="result">The object to be returned</param>
        /// <returns>This</returns>
        public Factory<TKey, TClass> Register(TKey key, TClass result) => Register(key, () => result);

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="key">Item to register</param>
        /// <param name="constructor">The function to call when creating the item</param>
        /// <returns>This</returns>
        public Factory<TKey, TClass> Register(TKey key, Func<TClass> constructor)
        {
            Constructors.SetValue(key, constructor);
            return this;
        }
    }
}