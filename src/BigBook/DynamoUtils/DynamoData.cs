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

namespace BigBook.DynamoUtils
{
    /// <summary>
    /// Dynamo data class
    /// </summary>
    internal class DynamoData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoData"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="data">The data.</param>
        /// <param name="version">The version.</param>
        internal DynamoData(DynamoClass definition, object?[] data, int version)
        {
            Version = version;
            Data = data;
            Definition = definition;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DynamoData"/> class from being created.
        /// </summary>
        private DynamoData()
        {
            Definition = DynamoClass.Empty;
            Data = Array.Empty<object>();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public object?[] Data { get; }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        public DynamoClass Definition { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; private set; }

        /// <summary>
        /// The empty object
        /// </summary>
        public static DynamoData Empty = new DynamoData();

        /// <summary>
        /// Gets or sets the <see cref="object?"/> at the specified index.
        /// </summary>
        /// <value>The <see cref="object?"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns>The value specified.</returns>
        public object? this[int index]
        {
            get
            {
                return Data[index];
            }
            set
            {
                ++Version;
                Data[index] = value;
            }
        }

        /// <summary>
        /// Updates the class.
        /// </summary>
        /// <param name="newClass">The new class.</param>
        /// <returns>The new DynamoData object.</returns>
        public DynamoData UpdateClass(DynamoClass newClass)
        {
            if (Data.Length >= newClass.Keys.Length)
            {
                this[newClass.Keys.Length - 1] = Dynamo.UninitializedObject;
                return new DynamoData(newClass, Data, Version);
            }
            else
            {
                int OldLength = Data.Length;
                object[] TempArray = new object[GetAlignedSize(newClass.Keys.Length)];
                Array.Copy(Data, TempArray, Data.Length);
                DynamoData NewData = new DynamoData(newClass, TempArray, Version);
                NewData[OldLength] = Dynamo.UninitializedObject;
                return NewData;
            }
        }

        /// <summary>
        /// Aligns the length to the value specified.
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns>The aligned size.</returns>
        private static int GetAlignedSize(int len)
        {
            const int DataAlignment = 8;
            return (len + (DataAlignment - 1)) & (~(DataAlignment - 1));
        }
    }
}