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

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigBook
{
    /// <summary>
    /// Process extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ProcessExtensions
    {
        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="process">Process to get information about</param>
        /// <param name="htmlFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this Process process, bool htmlFormat = true)
        {
            if (process == null)
            {
                return "";
            }

            var Builder = new StringBuilder();
            return Builder.Append(htmlFormat ? "<strong>" : "")
                   .Append(process.ProcessName)
                   .Append(" Information")
                   .Append(htmlFormat ? "</strong><br />" : "\n")
                   .Append(process.ToString(htmlFormat))
                   .Append(htmlFormat ? "<br />" : "\n")
                   .ToString();
        }

        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="processes">Processes to get information about</param>
        /// <param name="htmlFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this IEnumerable<Process> processes, bool htmlFormat = true)
        {
            if (processes == null)
            {
                return "";
            }

            var Builder = new StringBuilder();
            processes.ForEach(x => Builder.Append(x.GetInformation(htmlFormat)));
            return Builder.ToString();
        }

        /// <summary>
        /// Kills a process
        /// </summary>
        /// <param name="process">Process that should be killed</param>
        /// <param name="timeToKill">Amount of time (in ms) until the process is killed.</param>
        /// <returns>The input process</returns>
        public static async Task<Process> KillProcessAsync(this Process process, int timeToKill = 0)
        {
            if (process == null)
            {
                return null;
            }

            await Task.Run(() => KillProcessAsyncHelper(process, timeToKill)).ConfigureAwait(false);
            return process;
        }

        /// <summary>
        /// Kills a list of processes
        /// </summary>
        /// <param name="processes">Processes that should be killed</param>
        /// <param name="timeToKill">Amount of time (in ms) until the processes are killed.</param>
        /// <returns>The list of processes</returns>
        public static async Task<IEnumerable<Process>> KillProcessAsync(this IEnumerable<Process> processes, int timeToKill = 0)
        {
            if (processes?.Any() != true)
            {
                return new List<Process>();
            }

            await Task.Run(() => processes.ForEach(x => KillProcessAsyncHelper(x, timeToKill))).ConfigureAwait(false);
            return processes;
        }

        /// <summary>
        /// Kills a process asyncronously
        /// </summary>
        /// <param name="process">Process to kill</param>
        /// <param name="timeToKill">Amount of time until the process is killed</param>
        private static void KillProcessAsyncHelper(Process process, int timeToKill)
        {
            if (process == null)
            {
                return;
            }

            if (timeToKill > 0)
            {
                Thread.Sleep(timeToKill);
            }

            process.Kill();
        }
    }
}