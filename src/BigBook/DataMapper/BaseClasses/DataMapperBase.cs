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

using BigBook.DataMapper.Interfaces;
using System;
using System.Collections.Concurrent;

namespace BigBook.DataMapper.BaseClasses
{
    /// <summary>
    /// Data mapper base class
    /// </summary>
    public abstract class DataMapperBase : IDataMapper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected DataMapperBase()
        {
            Mappings = new ConcurrentDictionary<Tuple<Type, Type>, ITypeMapping>();
        }

        /// <summary>
        /// The name of the data mapper
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Mappings
        /// </summary>
        protected ConcurrentDictionary<Tuple<Type, Type>, ITypeMapping> Mappings { get; }

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping<Left, Right> Map<Left, Right>() => (ITypeMapping<Left, Right>)Map(typeof(Left), typeof(Right));

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping Map(Type left, Type right)
        {
            var Key = new Tuple<Type, Type>(left, right);
            if (Mappings.TryGetValue(Key, out var ReturnValue))
                return ReturnValue;
            var Key2 = new Tuple<Type, Type>(right, left);
            if (Mappings.TryGetValue(Key2, out ReturnValue))
            {
                ReturnValue = ReturnValue.CreateReversed();
                Mappings.AddOrUpdate(Key, ReturnValue, (_, y) => y);
            }
            else
            {
                ReturnValue = CreateTypeMapping(Key.Item1, Key.Item2);
                Mappings.AddOrUpdate(Key, ReturnValue, (_, y) => y);
            }
            return ReturnValue;
        }

        /// <summary>
        /// The name of the data mapper
        /// </summary>
        /// <returns>The name of the data mapper</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        protected abstract ITypeMapping<Left, Right> CreateTypeMapping<Left, Right>();

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        protected abstract ITypeMapping CreateTypeMapping(Type left, Type right);
    }
}