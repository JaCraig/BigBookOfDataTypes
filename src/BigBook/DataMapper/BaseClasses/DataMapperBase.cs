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
using System.Collections.Generic;

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
            Mappings = new Dictionary<int, ITypeMapping>();
            LockObject = new object();
        }

        /// <summary>
        /// The lock object
        /// </summary>
        internal readonly object LockObject;

        /// <summary>
        /// The name of the data mapper
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Mappings
        /// </summary>
        protected Dictionary<int, ITypeMapping> Mappings { get; }

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <typeparam name="TLeft">Left type</typeparam>
        /// <typeparam name="TRight">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping<TLeft, TRight>? Map<TLeft, TRight>() => Map(typeof(TLeft), typeof(TRight)) as ITypeMapping<TLeft, TRight>;

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping? Map(Type left, Type right)
        {
            if (left is null || right is null)
                return null;
            var Key = left.GetHashCode() ^ (right.GetHashCode() << 2);
            if (Mappings.TryGetValue(Key, out var ReturnValue))
                return ReturnValue;
            var Key2 = right.GetHashCode() ^ (left.GetHashCode() << 2);
            lock (LockObject)
            {
                var TempMappings = Mappings;
                if (TempMappings.TryGetValue(Key, out ReturnValue))
                    return ReturnValue;
                if (TempMappings.TryGetValue(Key2, out ReturnValue))
                {
                    ReturnValue = ReturnValue.CreateReversed();
                    TempMappings.Add(Key, ReturnValue);
                }
                else
                {
                    ReturnValue = CreateTypeMapping(left, right);
                    TempMappings.Add(Key, ReturnValue);
                }
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
        /// <typeparam name="TLeft">Left type</typeparam>
        /// <typeparam name="TRight">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        protected abstract ITypeMapping<TLeft, TRight> CreateTypeMapping<TLeft, TRight>();

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        protected abstract ITypeMapping CreateTypeMapping(Type left, Type right);
    }
}