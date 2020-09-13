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
using System.Threading;
using System.Threading.Tasks;

namespace BigBook
{
    /// <summary>
    /// Async helper.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// The task factory
        /// </summary>
        private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Runs the Func synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The result.</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            => TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();

        /// <summary>
        /// Runs the synchronously.
        /// </summary>
        /// <param name="func">The function.</param>
        public static void RunSync(this Func<Task> func)
            => TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
    }
}