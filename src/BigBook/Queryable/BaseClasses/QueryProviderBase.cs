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

using Fast.Activator;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BigBook.Queryable.BaseClasses
{
    /// <summary>
    /// Query provider base class
    /// </summary>
    /// <seealso cref="IQueryProvider"/>
    public abstract class QueryProviderBase : IQueryProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProviderBase"/> class.
        /// </summary>
        protected QueryProviderBase()
        {
        }

        /// <summary>
        /// Constructs an <see cref="T:System.Linq.IQueryable`1"/> object that can evaluate the
        /// query represented by a specified expression tree.
        /// </summary>
        /// <typeparam name="TElement">
        /// The type of the elements of the <see cref="T:System.Linq.IQueryable`1"/> that is returned.
        /// </typeparam>
        /// <param name="expression">An expression tree that represents a LINQ query.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IQueryable`1"/> that can evaluate the query represented by
        /// the specified expression tree.
        /// </returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new Query<TElement>(this, expression);

        /// <summary>
        /// Constructs an <see cref="T:System.Linq.IQueryable"/> object that can evaluate the query
        /// represented by a specified expression tree.
        /// </summary>
        /// <param name="expression">An expression tree that represents a LINQ query.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IQueryable"/> that can evaluate the query represented by the
        /// specified expression tree.
        /// </returns>
        public IQueryable? CreateQuery(Expression expression)
        {
            if (expression is null)
                return null;
            var ElementType = expression.Type.GetIEnumerableElementType();

            try
            {
                return (IQueryable)FastActivator.CreateInstance(typeof(Query<>).MakeGenericType(ElementType), new object[] { this, expression });
            }
            catch (TargetInvocationException Err)
            {
                throw Err.InnerException;
            }
        }

        /// <summary>
        /// Executes the specified expression.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The value that results from executing the specified query.</returns>
        public TElement Execute<TElement>(Expression expression) => (TElement)Execute(expression);

        /// <summary>
        /// Executes the query represented by a specified expression tree.
        /// </summary>
        /// <param name="expression">An expression tree that represents a LINQ query.</param>
        /// <returns>The value that results from executing the specified query.</returns>
        public abstract object Execute(Expression expression);

        /// <summary>
        /// Gets the query text.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The query as a string</returns>
        public abstract string GetQueryText(Expression expression);
    }
}