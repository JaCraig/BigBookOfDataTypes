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

using BigBook.DataMapper;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Dynamo factory
    /// </summary>
    public class DynamoFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoFactory"/> class.
        /// </summary>
        /// <param name="aopManager">The aop manager.</param>
        /// <param name="builderPool">The builder pool.</param>
        /// <param name="dataMapper">The data mapper.</param>
        public DynamoFactory(Aspectus.Aspectus? aopManager, ObjectPool<StringBuilder>? builderPool, Manager? dataMapper)
        {
            DataMapper = dataMapper;
            BuilderPool = builderPool;
            AopManager = aopManager;
        }

        /// <summary>
        /// Gets the aop manager.
        /// </summary>
        /// <value>The aop manager.</value>
        private Aspectus.Aspectus? AopManager { get; }

        /// <summary>
        /// Gets the builder pool.
        /// </summary>
        /// <value>The builder pool.</value>
        private ObjectPool<StringBuilder>? BuilderPool { get; }

        /// <summary>
        /// Gets the data mapper.
        /// </summary>
        /// <value>The data mapper.</value>
        private Manager? DataMapper { get; }

        /// <summary>
        /// Creates a Dynamo object.
        /// </summary>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <returns>The Dynamo object</returns>
        public Dynamo Create(bool useChangeLog)
        {
            return new Dynamo(useChangeLog, AopManager, BuilderPool, DataMapper);
        }

        /// <summary>
        /// Creates a Dynamo object.
        /// </summary>
        /// <param name="item">The item to populate the Dynamo object with.</param>
        /// <param name="useChangeLog">if set to <c>true</c> [use change log].</param>
        /// <returns>The Dynamo object</returns>
        public Dynamo Create(object? item, bool useChangeLog = false)
        {
            return new Dynamo(item, useChangeLog, AopManager, BuilderPool, DataMapper);
        }
    }
}