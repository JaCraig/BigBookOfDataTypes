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
    /// <typeparam name="Key">The "message" type</typeparam>
    /// <typeparam name="T">The class type that you want created</typeparam>
    public class Factory<Key, T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Factory()
        {
            Constructors = new Dictionary<Key, Func<T>>();
        }

        /// <summary>
        /// List of constructors/initializers
        /// </summary>
        protected Dictionary<Key, Func<T>> Constructors { get; }

        /// <summary>
        /// Creates an instance associated with the key
        /// </summary>
        /// <param name="key">Registered item</param>
        /// <returns>The type returned by the initializer</returns>
        public T Create(Key key) => Constructors.GetValue(key, () => default(T))();

        /// <summary>
        /// Determines if a key has been registered
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Exists(Key key) => Constructors.ContainsKey(key);

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="key">Item to register</param>
        /// <param name="result">The object to be returned</param>
        /// <returns>This</returns>
        public Factory<Key, T> Register(Key key, T result) => Register(key, () => result);

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="key">Item to register</param>
        /// <param name="constructor">The function to call when creating the item</param>
        /// <returns>This</returns>
        public Factory<Key, T> Register(Key key, Func<T> constructor)
        {
            Constructors.SetValue(key, constructor);
            return this;
        }
    }
}