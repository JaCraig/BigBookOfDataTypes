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

using System.ComponentModel;
using System.IO;
using System.Text;

namespace BigBook
{
    /// <summary>
    /// Extension methods for Streams
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StreamExtensions
    {
        /// <summary>
        /// Takes all of the data in the stream and returns it as a string
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <param name="encodingUsing">Encoding that the string should be in (defaults to UTF8)</param>
        /// <returns>A string containing the content of the stream</returns>
        public static string ReadAll(this Stream input, Encoding encodingUsing = null)
        {
            if (input == null)
            {
                return "";
            }

            return input.ReadAllBinary().ToString(encodingUsing);
        }

        /// <summary>
        /// Takes all of the data in the stream and returns it as an array of bytes
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <returns>A byte array</returns>
        public static byte[] ReadAllBinary(this Stream input)
        {
            if (input == null)
            {
                return new byte[0];
            }

            if (input is MemoryStream TempInput)
            {
                return TempInput.ToArray();
            }

            byte[] Buffer = new byte[4096];
            using (MemoryStream Temp = new MemoryStream())
            {
                while (true)
                {
                    var Count = input.Read(Buffer, 0, Buffer.Length);
                    if (Count <= 0)
                    {
                        return Temp.ToArray();
                    }
                    Temp.Write(Buffer, 0, Count);
                }
            }
        }
    }
}