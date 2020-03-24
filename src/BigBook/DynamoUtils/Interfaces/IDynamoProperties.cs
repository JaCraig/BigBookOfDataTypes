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

namespace BigBook.DynamoUtils.Interfaces
{
    /// <summary>
    /// Dynamo alt properties interface
    /// </summary>
    internal interface IDynamoProperties
    {
        /// <summary>
        /// Setups the values.
        /// </summary>
        void SetupValues();

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the value is returned, false otherwise.</returns>
        bool TryGetValue(Dynamo @object, string propertyName, out object? value);

        /// <summary>
        /// Tries the set value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="oldValue">The old value.</param>
        /// <returns>True if it is set, false otherwise.</returns>
        bool TrySetValue(Dynamo @object, string propertyName, object? value, out object? oldValue);
    }
}