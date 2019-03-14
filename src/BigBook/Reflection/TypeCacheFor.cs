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
using System.Reflection;

namespace BigBook.Reflection
{
    /// <summary>
    /// Type cache info
    /// </summary>
    /// <typeparam name="T">Type to cache.</typeparam>
    public static class TypeCacheFor<T>
    {
        /// <summary>
        /// The constructors
        /// </summary>
        public static readonly ConstructorInfo[] Constructors = typeof(T).GetConstructors();

        /// <summary>
        /// The fields
        /// </summary>
        public static readonly FieldInfo[] Fields = typeof(T).GetFields();

        /// <summary>
        /// The interfaces
        /// </summary>
        public static readonly Type[] Interfaces = typeof(T).GetInterfaces();

        /// <summary>
        /// The methods
        /// </summary>
        public static readonly MethodInfo[] Methods = typeof(T).GetMethods();

        /// <summary>
        /// The properties
        /// </summary>
        public static readonly PropertyInfo[] Properties = typeof(T).GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The type
        /// </summary>
        public static readonly Type Type = typeof(T);
    }
}