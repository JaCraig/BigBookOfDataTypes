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

using BigBook.DynamoUtils.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;

namespace BigBook.DynamoUtils
{
    /// <summary>
    /// Dynamo type information
    /// </summary>
    internal class DynamoTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoTypes"/> class.
        /// </summary>
        public DynamoTypes()
        {
            Types = new Dictionary<Type, IDynamoProperties>();
            LockObject = new object();
        }

        /// <summary>
        /// The lock object
        /// </summary>
        private readonly object LockObject;

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        private Dictionary<Type, IDynamoProperties> Types { get; }

        /// <summary>
        /// Setups the type.
        /// </summary>
        /// <param name="object">The @object.</param>
        public void SetupType(Dynamo @object)
        {
            var objectType = @object.GetType();
            if (Types.ContainsKey(objectType) || objectType == typeof(Dynamo))
                return;
            Log.Logger?.Debug("Entering SetupType lock");
            lock (LockObject)
            {
                var TempObject = (typeof(DynamoProperties<>).MakeGenericType(objectType).Create() as IDynamoProperties)!;
                TempObject.SetupValues();
                Types.Add(objectType, TempObject);
            }
            Log.Logger?.Debug("Leaving SetupType lock");
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the value is returned, false otherwise.</returns>
        public bool TryGetValue(Dynamo @object, string propertyName, out object? value)
        {
            var objectType = @object.GetType();
            if (!Types.ContainsKey(objectType))
            {
                value = null;
                return false;
            }
            var ReturnValue = Types[objectType].TryGetValue(@object, propertyName, out var TempValue);
            value = TempValue;
            return ReturnValue;
        }

        /// <summary>
        /// Tries the set value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is set, false otherwise.</returns>
        public bool TrySetValue(Dynamo @object, string propertyName, object? value, out object? oldValue)
        {
            var objectType = @object.GetType();
            if (!Types.ContainsKey(objectType))
            {
                oldValue = null;
                return false;
            }
            var ReturnValue = Types[objectType].TrySetValue(@object, propertyName, value, out var TempValue);
            oldValue = TempValue;
            return ReturnValue;
        }
    }
}