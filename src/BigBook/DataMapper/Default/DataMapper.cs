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

using BigBook.DataMapper.BaseClasses;
using BigBook.DataMapper.Interfaces;
using System;

namespace BigBook.DataMapper.Default
{
    /// <summary>
    /// Default data mapper
    /// </summary>
    public class DataMapper : DataMapperBase
    {
        /// <summary>
        /// The name of the data mapper
        /// </summary>
        public override string Name { get; } = "Default";

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        protected override ITypeMapping<Left, Right> CreateTypeMapping<Left, Right>() => new TypeMapping<Left, Right>();

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        protected override ITypeMapping CreateTypeMapping(Type left, Type right) => (ITypeMapping)typeof(TypeMapping<,>).MakeGenericType(left, right).Create()!;
    }
}