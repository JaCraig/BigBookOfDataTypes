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
    /// SqlDbType converter
    /// </summary>
    public class SqlDbTypeTypeConverter : TypeConverterBase<SqlDbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlDbTypeTypeConverter()
        {
            ConvertToTypes.Add(typeof(Type), SqlDbTypeToType);
            ConvertToTypes.Add(typeof(DbType), SqlDbTypeToDbType);
            ConvertFromTypes.Add(typeof(Type).GetType(), TypeToSqlDbType);
            ConvertFromTypes.Add(typeof(DbType), DbTypeToSqlDbType);
            Conversions = new ConcurrentDictionary<Type, DbType>();
            Conversions.AddOrUpdate(typeof(byte), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte?), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte?), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short?), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort?), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int?), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint?), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long?), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong?), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float?), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double?), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal?), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool?), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(string), DbType.String, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char?), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid?), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime?), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset?), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(TimeSpan), DbType.Time, (x, y) => y);
            Conversions.AddOrUpdate(typeof(TimeSpan?), DbType.Time, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte[]), DbType.Binary, (x, y) => y);
        }

        /// <summary>
        /// Conversions
        /// </summary>
        protected static ConcurrentDictionary<Type, DbType> Conversions { get; private set; }

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get { return new EnumConverter(typeof(SqlDbType)); } }

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

        private static object SqlDbTypeToType(object arg)
        {
            if (!(arg is SqlDbType Item))
            {
                return typeof(int);
            }
            if (Item == SqlDbType.Time)
                return typeof(TimeSpan);

            var Parameter = new SqlParameter
            {
                SqlDbType = Item
            };
            switch (Parameter.DbType)
            {
                case DbType.Byte:
                    return typeof(byte);

                case DbType.SByte:
                    return typeof(sbyte);

                case DbType.Int16:
                    return typeof(short);

                case DbType.UInt16:
                    return typeof(ushort);

                case DbType.Int32:
                    return typeof(int);

                case DbType.UInt32:
                    return typeof(uint);

                case DbType.Int64:
                    return typeof(long);

                case DbType.UInt64:
                    return typeof(ulong);

                case DbType.Single:
                    return typeof(float);

                case DbType.Double:
                    return typeof(double);

                case DbType.Decimal:
                    return typeof(decimal);

                case DbType.Boolean:
                    return typeof(bool);

                case DbType.String:
                    return typeof(string);

                case DbType.StringFixedLength:
                    return typeof(char);

                case DbType.Guid:
                    return typeof(Guid);

                case DbType.DateTime2:
                    return typeof(DateTime);

                case DbType.DateTime:
                    return typeof(DateTime);

                case DbType.DateTimeOffset:
                    return typeof(DateTimeOffset);

                case DbType.Binary:
                    return typeof(byte[]);

                case DbType.Time:
                    return typeof(TimeSpan);
            }

            return typeof(int);
        }

        private static object TypeToSqlDbType(object arg)
        {
            if (!(arg is Type TempValue))
            {
                return SqlDbType.Int;
            }

            DbType Item = DbType.Int32;
            if (TempValue.IsEnum)
            {
                TempValue = Enum.GetUnderlyingType(TempValue);
            }

            Item = Conversions.GetValue(TempValue, DbType.Int32);
            if (Item == DbType.Time)
                return SqlDbType.Time;
            var Parameter = new SqlParameter
            {
                DbType = Item
            };
            return Parameter.SqlDbType;
        }
    }
}