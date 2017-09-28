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
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BigBook
{
    /// <summary>
    /// Class that helps with running tasks in parallel on a set of objects (that will come in on an
    /// ongoing basis, think producer/consumer situations)
    /// </summary>
    /// <typeparam name="T">Object type to process</typeparam>
    public class TaskQueue<T> : BlockingCollection<T>, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity">
        /// Number of items that are allowed to be processed in the queue at one time
        /// </param>
        /// <param name="processItem">Action that is used to process each item</param>
        /// <param name="timeOut">The time out to wait between items to process.</param>
        /// <param name="handleError">
        /// Handles an exception if it occurs (defaults to eating the error)
        /// </param>
        public TaskQueue(int capacity, Func<T, bool> processItem, int timeOut = 100, Action<Exception, T> handleError = null)
            : base(new ConcurrentQueue<T>())
        {
            Capacity = capacity;
            TimeOut = timeOut;
            if (capacity < 1)
                capacity = 1;
            ProcessItem = processItem;
            HandleError = handleError.Check((x, y) => { });
            CancellationToken = new CancellationTokenSource();
            Tasks = new Task[capacity];
        }

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity { get; }

        /// <summary>
        /// Determines if it has been cancelled
        /// </summary>
        public bool IsCanceled => CancellationToken.IsCancellationRequested;

        /// <summary>
        /// Determines if it has completed all tasks
        /// </summary>
        public bool IsComplete => Tasks.All(x => x == null || x.IsCompleted);

        /// <summary>
        /// Gets the time out.
        /// </summary>
        /// <value>The time out.</value>
        public int TimeOut { get; }

        /// <summary>
        /// Token used to signal cancellation
        /// </summary>
        private CancellationTokenSource CancellationToken { get; set; }

        /// <summary>
        /// Called when an exception occurs when processing the queue
        /// </summary>
        private Action<Exception, T> HandleError { get; set; }

        /// <summary>
        /// Action used to process an individual item in the queue
        /// </summary>
        private Func<T, bool> ProcessItem { get; set; }

        /// <summary>
        /// Group of tasks that the queue uses
        /// </summary>
        private Task[] Tasks { get; set; }

        /// <summary>
        /// Cancels the queue from processing
        /// </summary>
        /// <param name="wait">
        /// Determines if the function should wait for the tasks to complete before returning
        /// </param>
        /// <returns>True if it is cancelled, false otherwise</returns>
        public bool Cancel(bool wait = false)
        {
            if (IsCompleted || IsCanceled)
                return true;
            CancellationToken.Cancel(false);
            if (wait)
                Task.WaitAll(Tasks);
            return true;
        }

        /// <summary>
        /// Adds the item to the queue to be processed
        /// </summary>
        /// <param name="item">Item to process</param>
        /// <returns>True if it is enqueued, false otherwise</returns>
        public bool Enqueue(T item)
        {
            if (IsCanceled)
                return false;
            Add(item);
            StartTasks(Capacity);
            return true;
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="disposing">
        /// True to dispose of all resources, false only disposes of native resources
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (Tasks != null)
            {
                Cancel(true);
                Tasks = null;
            }
            if (CancellationToken != null)
            {
                CancellationToken.Dispose();
                CancellationToken = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Processes the queue
        /// </summary>
        private void Process()
        {
            if (CancellationToken == null || ProcessItem == null)
                return;
            while (true)
            {
                T Item = default(T);
                try
                {
                    if (!TryTake(out Item, TimeOut, CancellationToken.Token))
                        break;
                    if (!ProcessItem(Item))
                        break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    HandleError(ex, Item);
                }
            }
        }

        /// <summary>
        /// Starts the tasks.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        private void StartTasks(int capacity)
        {
            for (int x = 0; x < capacity; ++x)
            {
                if (Tasks[x] == null || Tasks[x].IsCompleted || Tasks[x].IsCanceled)
                    Tasks[x] = Task.Factory.StartNew(Process);
            }
        }
    }
}