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

using BigBook.Queryable.BaseClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BigBook.Queryable
{
    /// <summary>
    /// Reusable query class
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class Query<T> :  IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{T}"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        public Query(QueryProviderBase provider)
        {
            InternalProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = Expression.Constant(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Query{T}"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        /// <exception cref="System.ArgumentNullException">expression or provider</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">expression</exception>
        public Query(QueryProviderBase provider, Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException(nameof(expression));
            }

            InternalProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression;
        }

        /// <summary>
        /// The internal provider
        /// </summary>
        private readonly QueryProviderBase InternalProvider;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated
        /// with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        Type IQueryable.ElementType { get; } = typeof(T);

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public IQueryProvider Provider => InternalProvider;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Provider.Execute(Expression)).GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Provider.Execute(Expression)).GetEnumerator();

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => InternalProvider.GetQueryText(Expression);
    }
}