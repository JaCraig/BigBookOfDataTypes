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
using System.Linq;

namespace BigBook.DataMapper
{
    /// <summary>
    /// Data mapper manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataMappers">The data mappers.</param>
        /// <param name="mapperModules">The mapper modules.</param>
        public Manager(IEnumerable<IDataMapper> dataMappers, IEnumerable<IMapperModule> mapperModules)
        {
            GenericObjectExtensions.DataManager = this;
            dataMappers ??= Array.Empty<IDataMapper>();
            DataMapper = dataMappers.FirstOrDefault(x => x.GetType().Assembly != typeof(Manager).Assembly) ?? new Default.DefaultDataMapper();
            mapperModules.ForEach(x => x.Map(this));
        }

        /// <summary>
        /// Data mapper
        /// </summary>
        private IDataMapper DataMapper { get; }

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <typeparam name="TLeft">Left type</typeparam>
        /// <typeparam name="TRight">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping<TLeft, TRight>? Map<TLeft, TRight>() => DataMapper.Map<TLeft, TRight>();

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping? Map(Type left, Type right) => DataMapper.Map(left, right);

        /// <summary>
        /// Outputs the string information about the manager
        /// </summary>
        /// <returns>The string info about the manager</returns>
        public override string ToString() => $"Data mapper: {DataMapper}\r\n";
    }
}