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
        /// Runs the task synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="asyncTask">The asynchronous task.</param>
        /// <returns>The result.</returns>
        public static TResult RunSync<TResult>(this Task<TResult> asyncTask)
        {
            return Task.Run(async () => await asyncTask.ConfigureAwait(false)).GetAwaiter().GetResult();
        }
    }
}