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

using BigBook.Conversion.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace BigBook.Conversion
{
    /// <summary>
    /// DbType converter
    /// </summary>
    public class DbTypeTypeConverter : TypeConverterBase<DbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DbTypeTypeConverter()
        {
            ConvertToTypes.Add(typeof(Type), DbTypeToType);
            ConvertToTypes.Add(typeof(SqlDbType), DbTypeToSqlDbType);
            ConvertFromTypes.Add(typeof(Type).GetType(), TypeToDbType);
            ConvertFromTypes.Add(typeof(SqlDbType), SqlDbTypeToDbType);
        }

        /// <summary>
        /// Conversions
        /// </summary>
        protected static Dictionary<Type, DbType> Conversions { get; } = new Dictionary<Type, DbType>
            {
                { typeof(byte), DbType.Byte },
                { typeof(byte?), DbType.Byte },
                { typeof(sbyte), DbType.Byte },
                { typeof(sbyte?), DbType.Byte },
                { typeof(short), DbType.Int16 },
                { typeof(short?), DbType.Int16 },
                { typeof(ushort), DbType.Int16 },
                { typeof(ushort?), DbType.Int16 },
                { typeof(int), DbType.Int32 },
                { typeof(int?), DbType.Int32 },
                { typeof(uint), DbType.Int32 },
                { typeof(uint?), DbType.Int32 },
                { typeof(long), DbType.Int64 },
                { typeof(long?), DbType.Int64 },
                { typeof(ulong), DbType.Int64 },
                { typeof(ulong?), DbType.Int64 },
                { typeof(float), DbType.Single },
                { typeof(float?), DbType.Single },
                { typeof(double), DbType.Double },
                { typeof(double?), DbType.Double },
                { typeof(decimal), DbType.Decimal },
                { typeof(decimal?), DbType.Decimal },
                { typeof(bool), DbType.Boolean },
                { typeof(bool?), DbType.Boolean },
                { typeof(string), DbType.String },
                { typeof(char), DbType.StringFixedLength },
                { typeof(char?), DbType.StringFixedLength },
                { typeof(Guid), DbType.Guid },
                { typeof(Guid?), DbType.Guid },
                { typeof(DateTime), DbType.DateTime2 },
                { typeof(DateTime?), DbType.DateTime2 },
                { typeof(DateTimeOffset), DbType.DateTimeOffset },
                { typeof(DateTimeOffset?), DbType.DateTimeOffset },
                { typeof(TimeSpan), DbType.Time },
                { typeof(Uri), DbType.String },
                { typeof(TimeSpan?), DbType.Time },
                { typeof(byte[]), DbType.Binary }
            };

        /// <summary>
        /// Gets the database type to type conversions.
        /// </summary>
        /// <value>The database type to type conversions.</value>
        protected static Dictionary<DbType, Type> DbTypeToTypeConversions { get; } = new Dictionary<DbType, Type>
            {
                { DbType.Byte , typeof(byte)},
                {DbType.SByte , typeof(sbyte)},
                {DbType.Int16 , typeof(short)},
                {DbType.UInt16 , typeof(ushort)},
                {DbType.Int32 , typeof(int)},
                {DbType.UInt32 , typeof(uint)},
                {DbType.Int64 , typeof(long)},
                {DbType.UInt64 , typeof(ulong)},
                {DbType.Single , typeof(float)},
                {DbType.Double , typeof(double)},
                {DbType.Decimal , typeof(decimal)},
                {DbType.Boolean , typeof(bool)},
                {DbType.String , typeof(string)},
                {DbType.StringFixedLength , typeof(char)},
                {DbType.Guid , typeof(Guid)},
                {DbType.DateTime2 , typeof(DateTime)},
                {DbType.DateTime , typeof(DateTime)},
                {DbType.DateTimeOffset , typeof(DateTimeOffset)},
                {DbType.Binary , typeof(byte[])},
                {DbType.Time , typeof(TimeSpan)},
            };

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get; } = new EnumConverter(typeof(DbType));

        /// <summary>
        /// Gets the type of the int.
        /// </summary>
        /// <value>The type of the int.</value>
        private static Type IntType { get; } = typeof(int);

        /// <summary>
        /// Databases the type of the type to SQL database.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object DbTypeToSqlDbType(object value)
        {
            if (!(value is DbType TempValue))
            {
                return SqlDbType.Int;
            }

            if (TempValue == DbType.Time)
                return SqlDbType.Time;
            var Parameter = new SqlParameter
            {
                DbType = TempValue
            };
            return Parameter.SqlDbType;
        }

        /// <summary>
        /// Databases the type of the type to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static object DbTypeToType(object value)
        {
            if (!(value is DbType TempValue))
            {
                return typeof(int);
            }

            return DbTypeToTypeConversions.TryGetValue(TempValue, out var returnValue) ? returnValue : IntType;
        }

        /// <summary>
        /// SQLs the type of the database type to database.
        /// </summary>
        /// <param name="sqlDbType">Type of the SQL database.</param>
        /// <returns></returns>
        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (!(sqlDbType is SqlDbType Temp))
            {
                return DbType.Int32;
            }

            if (Temp == SqlDbType.Time)
                return DbType.Time;

            var Parameter = new SqlParameter
            {
                SqlDbType = Temp
            };
            return Parameter.DbType;
        }

        /// <summary>
        /// Types the type of to database.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <returns></returns>
        private static object TypeToDbType(object Object)
        {
            if (!(Object is Type TempValue))
            {
                return DbType.Int32;
            }

            if (TempValue.IsEnum)
            {
                TempValue = Enum.GetUnderlyingType(TempValue);
            }
            return Conversions.TryGetValue(TempValue, out var returnValue) ? returnValue : (object)DbType.Int32;
        }
    }
}