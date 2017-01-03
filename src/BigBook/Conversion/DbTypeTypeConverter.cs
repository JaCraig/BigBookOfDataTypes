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
using System.Reflection;

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
        protected override TypeConverter InternalConverter => new EnumConverter(typeof(DbType));

        private static object DbTypeToSqlDbType(object value)
        {
            if (!(value is DbType))
                return SqlDbType.Int;
            var TempValue = (DbType)value;
            var Parameter = new SqlParameter();
            Parameter.DbType = TempValue;
            return Parameter.SqlDbType;
        }

        private static object DbTypeToType(object value)
        {
            if (!(value is DbType))
                return typeof(int);
            var TempValue = (DbType)value;
            switch (TempValue)
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

        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (!(sqlDbType is SqlDbType))
                return DbType.Int32;
            var Temp = (SqlDbType)sqlDbType;
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Temp;
            return Parameter.DbType;
        }

        private static object TypeToDbType(object Object)
        {
            var TempValue = Object as Type;
            if (TempValue == null)
                return DbType.Int32;
            if (TempValue.GetTypeInfo().IsEnum)
                TempValue = Enum.GetUnderlyingType(TempValue);
            if (TempValue == typeof(byte)) return DbType.Byte;
            else if (TempValue == typeof(sbyte)) return DbType.SByte;
            else if (TempValue == typeof(short)) return DbType.Int16;
            else if (TempValue == typeof(ushort)) return DbType.UInt16;
            else if (TempValue == typeof(int)) return DbType.Int32;
            else if (TempValue == typeof(uint)) return DbType.UInt32;
            else if (TempValue == typeof(long)) return DbType.Int64;
            else if (TempValue == typeof(ulong)) return DbType.UInt64;
            else if (TempValue == typeof(float)) return DbType.Single;
            else if (TempValue == typeof(double)) return DbType.Double;
            else if (TempValue == typeof(decimal)) return DbType.Decimal;
            else if (TempValue == typeof(bool)) return DbType.Boolean;
            else if (TempValue == typeof(string)) return DbType.String;
            else if (TempValue == typeof(char)) return DbType.StringFixedLength;
            else if (TempValue == typeof(Guid)) return DbType.Guid;
            else if (TempValue == typeof(DateTime)) return DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (TempValue == typeof(TimeSpan)) return DbType.Time;
            else if (TempValue == typeof(byte[])) return DbType.Binary;
            else if (TempValue == typeof(byte?)) return DbType.Byte;
            else if (TempValue == typeof(sbyte?)) return DbType.SByte;
            else if (TempValue == typeof(short?)) return DbType.Int16;
            else if (TempValue == typeof(ushort?)) return DbType.UInt16;
            else if (TempValue == typeof(int?)) return DbType.Int32;
            else if (TempValue == typeof(uint?)) return DbType.UInt32;
            else if (TempValue == typeof(long?)) return DbType.Int64;
            else if (TempValue == typeof(ulong?)) return DbType.UInt64;
            else if (TempValue == typeof(float?)) return DbType.Single;
            else if (TempValue == typeof(double?)) return DbType.Double;
            else if (TempValue == typeof(decimal?)) return DbType.Decimal;
            else if (TempValue == typeof(bool?)) return DbType.Boolean;
            else if (TempValue == typeof(char?)) return DbType.StringFixedLength;
            else if (TempValue == typeof(Guid?)) return DbType.Guid;
            else if (TempValue == typeof(DateTime?)) return DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            else if (TempValue == typeof(TimeSpan?)) return DbType.Time;
            return DbType.Int32;
        }
    }
}