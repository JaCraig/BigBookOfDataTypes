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
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get; } = new EnumConverter(typeof(DbType));

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

            if (TempValue == typeof(byte))
            {
                return DbType.Byte;
            }
            else if (TempValue == typeof(sbyte))
            {
                return DbType.Byte;
            }
            else if (TempValue == typeof(short))
            {
                return DbType.Int16;
            }
            else if (TempValue == typeof(ushort))
            {
                return DbType.Int16;
            }
            else if (TempValue == typeof(int))
            {
                return DbType.Int32;
            }
            else if (TempValue == typeof(uint))
            {
                return DbType.Int32;
            }
            else if (TempValue == typeof(long))
            {
                return DbType.Int64;
            }
            else if (TempValue == typeof(ulong))
            {
                return DbType.Int64;
            }
            else if (TempValue == typeof(float))
            {
                return DbType.Single;
            }
            else if (TempValue == typeof(double))
            {
                return DbType.Double;
            }
            else if (TempValue == typeof(decimal))
            {
                return DbType.Decimal;
            }
            else if (TempValue == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (TempValue == typeof(string))
            {
                return DbType.String;
            }
            else if (TempValue == typeof(char))
            {
                return DbType.StringFixedLength;
            }
            else if (TempValue == typeof(Guid))
            {
                return DbType.Guid;
            }
            else if (TempValue == typeof(DateTime))
            {
                return DbType.DateTime2;
            }
            else if (TempValue == typeof(DateTimeOffset))
            {
                return DbType.DateTimeOffset;
            }
            else if (TempValue == typeof(TimeSpan))
            {
                return DbType.Time;
            }
            else if (TempValue == typeof(byte[]))
            {
                return DbType.Binary;
            }
            else if (TempValue == typeof(byte?))
            {
                return DbType.Byte;
            }
            else if (TempValue == typeof(sbyte?))
            {
                return DbType.Byte;
            }
            else if (TempValue == typeof(short?))
            {
                return DbType.Int16;
            }
            else if (TempValue == typeof(ushort?))
            {
                return DbType.Int16;
            }
            else if (TempValue == typeof(int?))
            {
                return DbType.Int32;
            }
            else if (TempValue == typeof(uint?))
            {
                return DbType.Int32;
            }
            else if (TempValue == typeof(long?))
            {
                return DbType.Int64;
            }
            else if (TempValue == typeof(ulong?))
            {
                return DbType.Int64;
            }
            else if (TempValue == typeof(float?))
            {
                return DbType.Single;
            }
            else if (TempValue == typeof(double?))
            {
                return DbType.Double;
            }
            else if (TempValue == typeof(decimal?))
            {
                return DbType.Decimal;
            }
            else if (TempValue == typeof(bool?))
            {
                return DbType.Boolean;
            }
            else if (TempValue == typeof(char?))
            {
                return DbType.StringFixedLength;
            }
            else if (TempValue == typeof(Guid?))
            {
                return DbType.Guid;
            }
            else if (TempValue == typeof(DateTime?))
            {
                return DbType.DateTime2;
            }
            else if (TempValue == typeof(DateTimeOffset?))
            {
                return DbType.DateTimeOffset;
            }
            else if (TempValue == typeof(TimeSpan?))
            {
                return DbType.Time;
            }

            return DbType.Int32;
        }
    }
}