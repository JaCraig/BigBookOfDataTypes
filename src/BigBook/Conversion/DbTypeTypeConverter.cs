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
using System.Collections.Concurrent;
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
            Conversions = new ConcurrentDictionary<Type, DbType>();
            Conversions.AddOrUpdate(typeof(byte), DbType.Byte, (_, y) => y);
            Conversions.AddOrUpdate(typeof(byte?), DbType.Byte, (_, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte), DbType.Byte, (_, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte?), DbType.Byte, (_, y) => y);
            Conversions.AddOrUpdate(typeof(short), DbType.Int16, (_, y) => y);
            Conversions.AddOrUpdate(typeof(short?), DbType.Int16, (_, y) => y);
            Conversions.AddOrUpdate(typeof(ushort), DbType.Int16, (_, y) => y);
            Conversions.AddOrUpdate(typeof(ushort?), DbType.Int16, (_, y) => y);
            Conversions.AddOrUpdate(typeof(int), DbType.Int32, (_, y) => y);
            Conversions.AddOrUpdate(typeof(int?), DbType.Int32, (_, y) => y);
            Conversions.AddOrUpdate(typeof(uint), DbType.Int32, (_, y) => y);
            Conversions.AddOrUpdate(typeof(uint?), DbType.Int32, (_, y) => y);
            Conversions.AddOrUpdate(typeof(long), DbType.Int64, (_, y) => y);
            Conversions.AddOrUpdate(typeof(long?), DbType.Int64, (_, y) => y);
            Conversions.AddOrUpdate(typeof(ulong), DbType.Int64, (_, y) => y);
            Conversions.AddOrUpdate(typeof(ulong?), DbType.Int64, (_, y) => y);
            Conversions.AddOrUpdate(typeof(float), DbType.Single, (_, y) => y);
            Conversions.AddOrUpdate(typeof(float?), DbType.Single, (_, y) => y);
            Conversions.AddOrUpdate(typeof(double), DbType.Double, (_, y) => y);
            Conversions.AddOrUpdate(typeof(double?), DbType.Double, (_, y) => y);
            Conversions.AddOrUpdate(typeof(decimal), DbType.Decimal, (_, y) => y);
            Conversions.AddOrUpdate(typeof(decimal?), DbType.Decimal, (_, y) => y);
            Conversions.AddOrUpdate(typeof(bool), DbType.Boolean, (_, y) => y);
            Conversions.AddOrUpdate(typeof(bool?), DbType.Boolean, (_, y) => y);
            Conversions.AddOrUpdate(typeof(string), DbType.String, (_, y) => y);
            Conversions.AddOrUpdate(typeof(char), DbType.StringFixedLength, (_, y) => y);
            Conversions.AddOrUpdate(typeof(char?), DbType.StringFixedLength, (_, y) => y);
            Conversions.AddOrUpdate(typeof(Guid), DbType.Guid, (_, y) => y);
            Conversions.AddOrUpdate(typeof(Guid?), DbType.Guid, (_, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime), DbType.DateTime2, (_, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime?), DbType.DateTime2, (_, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset), DbType.DateTimeOffset, (_, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset?), DbType.DateTimeOffset, (_, y) => y);
            Conversions.AddOrUpdate(typeof(TimeSpan), DbType.Time, (_, y) => y);
            Conversions.AddOrUpdate(typeof(Uri), DbType.String, (_, y) => y);
            Conversions.AddOrUpdate(typeof(TimeSpan?), DbType.Time, (_, y) => y);
            Conversions.AddOrUpdate(typeof(byte[]), DbType.Binary, (_, y) => y);
        }

        /// <summary>
        /// Conversions
        /// </summary>
        protected static ConcurrentDictionary<Type, DbType>? Conversions { get; private set; }

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get; } = new EnumConverter(typeof(DbType));

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

            return TempValue switch
            {
                DbType.Byte => typeof(byte),

                DbType.SByte => typeof(sbyte),

                DbType.Int16 => typeof(short),

                DbType.UInt16 => typeof(ushort),

                DbType.Int32 => typeof(int),

                DbType.UInt32 => typeof(uint),

                DbType.Int64 => typeof(long),

                DbType.UInt64 => typeof(ulong),

                DbType.Single => typeof(float),

                DbType.Double => typeof(double),

                DbType.Decimal => typeof(decimal),

                DbType.Boolean => typeof(bool),

                DbType.String => typeof(string),

                DbType.StringFixedLength => typeof(char),

                DbType.Guid => typeof(Guid),

                DbType.DateTime2 => typeof(DateTime),

                DbType.DateTime => typeof(DateTime),

                DbType.DateTimeOffset => typeof(DateTimeOffset),

                DbType.Binary => typeof(byte[]),

                DbType.Time => typeof(TimeSpan),

                _ => typeof(int),
            };
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

            return Conversions?.GetValue(TempValue, DbType.Int32) ?? DbType.Int32;
        }
    }
}