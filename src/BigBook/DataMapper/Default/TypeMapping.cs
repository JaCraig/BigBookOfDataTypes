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
using System.Linq.Expressions;

namespace BigBook.DataMapper.Default
{
    /// <summary>
    /// Type mapping default class
    /// </summary>
    /// <typeparam name="Left">The type of the eft.</typeparam>
    /// <typeparam name="Right">The type of the ight.</typeparam>
    /// <seealso cref="BigBook.DataMapper.BaseClasses.TypeMappingBase{Left, Right}"/>
    public class TypeMapping<Left, Right> : TypeMappingBase<Left, Right>
    {
        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Expression<Func<Right, object>> rightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(leftExpression, rightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Expression<Func<Right, object>> rightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(leftGet, leftSet, rightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Func<Right, object> rightGet, Action<Right, object> rightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(leftExpression, rightGet, rightSet));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Func<Right, object> rightGet, Action<Right, object> rightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(leftGet, leftSet, rightGet, rightSet));
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public override void Copy(Left source, Right destination)
        {
            foreach (var TempMapping in Mappings)
            {
                TempMapping.Copy(source, destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public override void Copy(Right source, Left destination)
        {
            foreach (var TempMapping in Mappings)
            {
                TempMapping.Copy(source, destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right are
        /// the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public override void CopyLeftToRight(Left source, Right destination)
        {
            Copy(source, destination);
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right are
        /// the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public override void CopyRightToLeft(Right source, Left destination)
        {
            Copy(source, destination);
        }

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The type mapping</returns>
        public override ITypeMapping CreateReversed()
        {
            var Result = new TypeMapping<Right, Left>();
            foreach (var Mapping in Mappings)
            {
                Result.Mappings.Add((IMapping<Right, Left>)Mapping.CreateReversed());
            }
            return Result;
        }
    }
}