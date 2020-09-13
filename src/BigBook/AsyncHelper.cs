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