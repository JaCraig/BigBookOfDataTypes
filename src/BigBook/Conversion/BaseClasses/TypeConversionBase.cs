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

using BigBook.Conversion.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace BigBook.Conversion.BaseClasses
{
    /// <summary>
    /// Type converter base class
    /// </summary>
    /// <typeparam name="T">Converter type</typeparam>
    public abstract class TypeConverterBase<T> : TypeConverter, IConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TypeConverterBase()
        {
            ConvertToTypes = new Dictionary<Type, Func<object, object>>();
            ConvertFromTypes = new Dictionary<Type, Func<object, object>>();
            AssociatedType = typeof(T);
        }

        /// <summary>
        /// Associated type
        /// </summary>
        public Type AssociatedType { get; }

        /// <summary>
        /// Types it can convert from and mapped functions
        /// </summary>
        protected IDictionary<Type, Func<object, object>> ConvertFromTypes { get; }

        /// <summary>
        /// Types it can convert to and mapped functions
        /// </summary>
        protected IDictionary<Type, Func<object, object>> ConvertToTypes { get; }

        /// <summary>
        /// Converter used internally if this can not convert the object
        /// </summary>
        protected abstract TypeConverter InternalConverter { get; }

        /// <summary>
        /// Can convert from
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="sourceType">Source type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => ConvertFromTypes.Keys.Contains(sourceType) || base.CanConvertFrom(context, sourceType);

        /// <summary>
        /// Can convert to
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => ConvertToTypes.Keys.Contains(destinationType) || base.CanConvertTo(context, destinationType);

        /// <summary>
        /// Convert from an object to a DbType
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value">Value</param>
        /// <returns>The DbType version</returns>
        public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            var ValueType = value.GetType();
            if (ConvertFromTypes.ContainsKey(ValueType))
            {
                return ConvertFromTypes[ValueType](value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the DbType object to another type
        /// </summary>
        /// <param name="context">Context type</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            if (ConvertToTypes.ContainsKey(destinationType))
            {
                return ConvertToTypes[destinationType](value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}