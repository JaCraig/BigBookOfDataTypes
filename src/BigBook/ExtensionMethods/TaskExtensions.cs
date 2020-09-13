using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BigBook
{
    /// <summary>
    /// Task extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TaskExtensions
    {
        /// <summary>
        /// Runs the Func synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The result.</returns>
        public static TResult RunSync<TResult>(this Func<Task<TResult>> func) => AsyncHelper.RunSync(func);

        /// <summary>
        /// Runs the synchronously.
        /// </summary>
        /// <param name="func">The function.</param>
        public static void RunSync(this Func<Task> func) => AsyncHelper.RunSync(func);
    }
}