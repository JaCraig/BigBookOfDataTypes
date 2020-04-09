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
    /// Mapping class
    /// </summary>
    /// <typeparam name="TLeft">Left type</typeparam>
    /// <typeparam name="TRight">Right type</typeparam>
    public class Mapping<TLeft, TRight> : MappingBase<TLeft, TRight>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        public Mapping(Expression<Func<TLeft, object>>? leftExpression, Expression<Func<TRight, object>>? rightExpression)
            : this(leftExpression?.Compile(),
                    leftExpression?.PropertySetter<TLeft>()?.Compile(),
                    rightExpression?.Compile(),
                    rightExpression?.PropertySetter<TRight>()?.Compile())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        public Mapping(Func<TLeft, object>? leftGet, Action<TLeft, object>? leftSet, Expression<Func<TRight, object>>? rightExpression)
            : this(leftGet,
                    leftSet,
                    rightExpression?.Compile(),
                    rightExpression?.PropertySetter<TRight>()?.Compile())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        public Mapping(Expression<Func<TLeft, object>>? leftExpression, Func<TRight, object>? rightGet, Action<TRight, object>? rightSet)
            : this(leftExpression?.Compile(),
                    leftExpression?.PropertySetter<TLeft>()?.Compile(),
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
        public Mapping(Func<TLeft, object>? leftGet, Action<TLeft, object>? leftSet, Func<TRight, object>? rightGet, Action<TRight, object>? rightSet)
        {
            LeftGet = leftGet;
            LeftSet = leftSet ?? DefaultLeftSet;
            RightGet = rightGet;
            RightSet = rightSet ?? DefaultRightSet;
        }

        /// <summary>
        /// Left get function
        /// </summary>
        protected Func<TLeft, object>? LeftGet { get; set; }

        /// <summary>
        /// Left set function
        /// </summary>
        protected Action<TLeft, object>? LeftSet { get; set; }

        /// <summary>
        /// Right get function
        /// </summary>
        protected Func<TRight, object>? RightGet { get; set; }

        /// <summary>
        /// Right set function
        /// </summary>
        protected Action<TRight, object>? RightSet { get; set; }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public override void Copy(TLeft source, TRight destination)
        {
            if (LeftGet is null || RightSet is null)
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
        public override void Copy(TRight source, TLeft destination)
        {
            if (RightGet is null || LeftSet is null)
            {
                return;
            }

            LeftSet(destination, RightGet(source));
        }

        /// <summary>
        /// Creates the reversed.
        /// </summary>
        /// <returns>The mapping.</returns>
        public override IMapping CreateReversed() => new Mapping<TRight, TLeft>(RightGet, RightSet, LeftGet, LeftSet);

        /// <summary>
        /// Default left set.
        /// </summary>
        /// <param name="_">The .</param>
        /// <param name="__">The .</param>
        private static void DefaultLeftSet(TLeft _, object __) { }

        /// <summary>
        /// Default right set.
        /// </summary>
        /// <param name="_">The .</param>
        /// <param name="__">The .</param>
        private static void DefaultRightSet(TRight _, object __) { }
    }
}