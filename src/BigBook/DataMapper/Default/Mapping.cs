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
using System;
using System.Linq.Expressions;

namespace BigBook.DataMapper.Default
{
    /// <summary>
    /// Mapping class
    /// </summary>
    /// <typeparam name="Left">Left type</typeparam>
    /// <typeparam name="Right">Right type</typeparam>
    public class Mapping<Left, Right> : MappingBase<Left, Right>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        public Mapping(Expression<Func<Left, object>> leftExpression, Expression<Func<Right, object>> rightExpression)
            : this(leftExpression?.Compile(),
                    leftExpression?.PropertySetter<Left>().Compile(),
                    rightExpression?.Compile(),
                    rightExpression?.PropertySetter<Right>().Compile())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        public Mapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Expression<Func<Right, object>> rightExpression)
            : this(leftGet,
                    leftSet,
                    rightExpression?.Compile(),
                    rightExpression?.PropertySetter<Right>().Compile())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        public Mapping(Expression<Func<Left, object>> leftExpression, Func<Right, object> rightGet, Action<Right, object> rightSet)
            : this(leftExpression?.Compile(),
                    leftExpression?.PropertySetter<Left>().Compile(),
                    rightGet,
                    rightSet)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        public Mapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Func<Right, object> rightGet, Action<Right, object> rightSet)
        {
            LeftGet = leftGet;
            LeftSet = leftSet.Check((_, __) => { });
            RightGet = rightGet;
            RightSet = rightSet.Check((_, __) => { });
        }

        /// <summary>
        /// Left get function
        /// </summary>
        protected Func<Left, object> LeftGet { get; set; }

        /// <summary>
        /// Left set function
        /// </summary>
        protected Action<Left, object> LeftSet { get; set; }

        /// <summary>
        /// Right get function
        /// </summary>
        protected Func<Right, object> RightGet { get; set; }

        /// <summary>
        /// Right set function
        /// </summary>
        protected Action<Right, object> RightSet { get; set; }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public virtual void Copy(Left source, Right destination)
        {
            if (LeftGet == null)
            {
                return;
            }

            RightSet(destination, LeftGet(source));
        }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public virtual void Copy(Right source, Left destination)
        {
            if (RightGet == null)
            {
                return;
            }

            LeftSet(destination, RightGet(source));
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right are
        /// the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public virtual void CopyLeftToRight(Left source, Right destination)
        {
            if (LeftGet == null)
            {
                return;
            }

            RightSet(destination, LeftGet(source));
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right are
        /// the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public virtual void CopyRightToLeft(Right source, Left destination)
        {
            if (RightGet == null)
            {
                return;
            }

            LeftSet(destination, RightGet(source));
        }
    }
}