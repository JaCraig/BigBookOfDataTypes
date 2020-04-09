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

namespace BigBook.DataMapper.Interfaces
{
    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The reverse mapping</returns>
        IMapping CreateReversed();
    }

    /// <summary>
    /// Mapping interface
    /// </summary>
    /// <typeparam name="TLeft">Left type</typeparam>
    /// <typeparam name="TRight">Right type</typeparam>
    public interface IMapping<TLeft, TRight> : IMapping
    {
        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        void Copy(TLeft source, TRight destination);

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        void Copy(TRight source, TLeft destination);
    }
}