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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BigBook
{
    /// <summary>
    /// Extension methods related to the stack trace
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StackTraceExtensions
    {
        /// <summary>
        /// Gets the methods involved in the stack trace
        /// </summary>
        /// <param name="stack">Stack trace to get methods from</param>
        /// <param name="excludedAssemblies">Excludes methods from the specified assemblies</param>
        /// <returns>A list of methods involved in the stack trace</returns>
        public static IEnumerable<MethodBase> GetMethods(this StackTrace stack, params Assembly[] excludedAssemblies)
        {
            if (stack == null)
            {
                return Array.Empty<MethodBase>();
            }

            excludedAssemblies ??= Array.Empty<Assembly>();
            return stack.GetFrames().GetMethods(excludedAssemblies);
        }

        /// <summary>
        /// Gets the methods involved in the individual frames
        /// </summary>
        /// <param name="frames">Frames to get the methods from</param>
        /// <param name="excludedAssemblies">Excludes methods from the specified assemblies</param>
        /// <returns>The list of methods involved</returns>
        public static IEnumerable<MethodBase> GetMethods(this IEnumerable<StackFrame> frames, params Assembly[] excludedAssemblies)
        {
            var Methods = new List<MethodBase>();
            if (frames == null)
            {
                return Methods;
            }

            foreach (var Frame in frames)
            {
                Methods.AddIf(x => x.DeclaringType != null
                    && !excludedAssemblies.Contains(x.DeclaringType.Assembly)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("System", StringComparison.Ordinal)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("mscorlib", StringComparison.Ordinal)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("WebDev.WebHost40", StringComparison.Ordinal),
                        Frame.GetMethod());
            }
            return Methods;
        }
    }
}