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
using BigBook.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BigBook.DynamoUtils
{
    /// <summary>
    /// DynamoProperties
    /// </summary>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    internal class DynamoProperties<TClass> : IDynamoProperties
        where TClass : Dynamo<TClass>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoProperties{TClass}"/> class.
        /// </summary>
        public DynamoProperties()
        {
            GetProperties = new Dictionary<int, Func<TClass, object?>>();
            SetProperties = new Dictionary<int, Action<TClass, object?>>();
        }

        /// <summary>
        /// Gets or sets the get properties.
        /// </summary>
        /// <value>The get properties.</value>
        public Dictionary<int, Func<TClass, object?>> GetProperties { get; set; }

        /// <summary>
        /// Gets or sets the set properties.
        /// </summary>
        /// <value>The set properties.</value>
        public Dictionary<int, Action<TClass, object?>> SetProperties { get; set; }

        /// <summary>
        /// Setups the values.
        /// </summary>
        /// <returns>This.</returns>
        public void SetupValues()
        {
            if (GetProperties.Count > 0 || SetProperties.Count > 0)
                return;
            foreach (var Property in TypeCacheFor<TClass>.Properties.Where(x => x.GetIndexParameters().Length == 0))
            {
                var Key = Property.Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
                if (Property.CanRead)
                    GetProperties.Add(Key, Property.PropertyGetter<TClass>().Compile());
                if (Property.CanWrite)
                    SetProperties.Add(Key, Property.PropertySetter<TClass, object>()?.Compile()!);
            }
        }

        /// <summary>
        /// Tries to get the value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is a property, false otherwise.</returns>
        public bool TryGetValue(Dynamo @object, string propertyName, out object? value)
        {
            if (@object is null)
            {
                value = null;
                return false;
            }
            var Key = propertyName.GetHashCode(StringComparison.OrdinalIgnoreCase);
            if (!GetProperties.ContainsKey(Key))
            {
                value = null;
                return false;
            }

            value = GetProperties[Key]((@object as TClass)!);
            return true;
        }

        /// <summary>
        /// Tries to set the value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <returns>True if it is set, false otherwise.</returns>
        public bool TrySetValue(Dynamo @object, string propertyName, object? value, out object? oldValue)
        {
            if (@object is null)
            {
                oldValue = null;
                return false;
            }
            var Key = propertyName.GetHashCode(StringComparison.OrdinalIgnoreCase);
            if (!SetProperties.ContainsKey(Key))
            {
                oldValue = null;
                return false;
            }
            if (!(@object is TClass TempObject))
            {
                oldValue = null;
                return false;
            }
            TryGetValue(TempObject, Key, out var TempValue);
            oldValue = TempValue;
            SetProperties[Key](TempObject, value);
            return true;
        }

        /// <summary>
        /// Tries to get the value.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if it is found, false otherwise.</returns>
        private bool TryGetValue(TClass @object, int key, out object? value)
        {
            if (!GetProperties.ContainsKey(key))
            {
                value = null;
                return false;
            }

            value = GetProperties[key](@object);
            return true;
        }
    }
}