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

namespace BigBook.DataMapper.BaseClasses
{
    /// <summary>
    /// Mapping base class
    /// </summary>
    /// <typeparam name="TLeft">The type of the eft.</typeparam>
    /// <typeparam name="TRight">The type of the ight.</typeparam>
    /// <seealso cref="Interfaces.IMapping{Left, Right}"/>
    public abstract class MappingBase<TLeft, TRight> : IMapping<TLeft, TRight>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MappingBase()
        {
        }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(TLeft source, TRight destination);

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public abstract void Copy(TRight source, TLeft destination);

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The mapping.</returns>
        public abstract IMapping CreateReversed();
    }
}