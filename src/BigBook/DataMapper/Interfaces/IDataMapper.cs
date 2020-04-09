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

namespace BigBook.DataMapper.Interfaces
{
    /// <summary>
    /// Data mapper interface
    /// </summary>
    public interface IDataMapper
    {
        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <typeparam name="TLeft">Left type</typeparam>
        /// <typeparam name="TRight">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        ITypeMapping<TLeft, TRight>? Map<TLeft, TRight>();

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <param name="left">Left type</param>
        /// <param name="right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        ITypeMapping? Map(Type left, Type right);
    }
}