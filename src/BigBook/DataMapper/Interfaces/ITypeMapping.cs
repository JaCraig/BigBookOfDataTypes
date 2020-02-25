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
using System.Linq.Expressions;

namespace BigBook.DataMapper.Interfaces
{
    /// <summary>
    /// Type mapping interface
    /// </summary>
    public interface ITypeMapping
    {
        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        ITypeMapping AutoMap();

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        void Copy(object source, object destination);

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The type mapping</returns>
        ITypeMapping CreateReversed();
    }

    /// <summary>
    /// Type mapping interface
    /// </summary>
    /// <typeparam name="TLeft">Left type</typeparam>
    /// <typeparam name="TRight">Right type</typeparam>
    public interface ITypeMapping<TLeft, TRight> : ITypeMapping
    {
        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        ITypeMapping<TLeft, TRight> AddMapping(Expression<Func<TLeft, object>> leftExpression, Expression<Func<TRight, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        ITypeMapping<TLeft, TRight> AddMapping(Func<TLeft, object> leftGet, Action<TLeft, object> leftSet, Expression<Func<TRight, object>> rightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        ITypeMapping<TLeft, TRight> AddMapping(Expression<Func<TLeft, object>> leftExpression, Func<TRight, object> rightGet, Action<TRight, object> rightSet);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        ITypeMapping<TLeft, TRight> AddMapping(Func<TLeft, object> leftGet, Action<TLeft, object> leftSet, Func<TRight, object> rightGet, Action<TRight, object> rightSet);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        void Copy(TLeft source, TRight destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        void Copy(TRight source, TLeft destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        void CopyLeftToRight(TLeft source, TRight destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        void CopyRightToLeft(TRight source, TLeft destination);
    }
}