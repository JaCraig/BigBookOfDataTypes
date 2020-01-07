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

namespace BigBook.Patterns.BaseClasses
{
    /// <summary>
    /// String enum base class
    /// </summary>
    /// <typeparam name="TClass">The type of the class.</typeparam>
    public abstract class StringEnumBaseClass<TClass>
        where TClass : StringEnumBaseClass<TClass>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEnumBaseClass{TClass}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected StringEnumBaseClass(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        protected string Name { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="StringEnumBaseClass{TClass}"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(StringEnumBaseClass<TClass>? enumType)
        {
            return enumType?.ToString() ?? "";
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="StringEnumBaseClass{TClass}"/>.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator StringEnumBaseClass<TClass>(string? enumType)
        {
            return new TClass { Name = enumType ?? "" };
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => Name;
    }
}