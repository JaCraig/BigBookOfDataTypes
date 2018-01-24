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
using System.ComponentModel;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Class for housing exception specific extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the exception to a string and appends the specified prefix/suffix (used for logging)
        /// </summary>
        /// <param name="exception">Exception to convert</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="suffix">Suffix</param>
        /// <returns>The exception as a string</returns>
        public static string ToString(this Exception exception, string prefix, string suffix = "")
        {
            if (exception == null)
                return "";
            var Builder = new StringBuilder();
            Builder.AppendLine(prefix);
            Builder.AppendLineFormat("Exception: {0}", exception.Message)
                   .AppendLineFormat("Exception Type: {0}", exception.GetType().FullName);
            if (exception.Data != null)
            {
                for (int x = 0, exceptionDataCount = exception.Data.Count; x < exceptionDataCount; x++)
                {
                    object Object = exception.Data[x];
                    Builder.AppendLineFormat("Data: {0}:{1}", Object, exception.Data[Object]);
                }
            }
            Builder.AppendLineFormat("StackTrace: {0}", exception.StackTrace)
                   .AppendLineFormat("Source: {0}", exception.Source);
            if (exception.InnerException != null)
                Builder.Append(exception.InnerException.ToString(prefix, suffix));
            Builder.AppendLine(suffix);
            return Builder.ToString();
        }
    }
}