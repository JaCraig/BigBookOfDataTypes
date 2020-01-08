using System;
using System.Data;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class DbTypeTypeConverterTests
    {
        public static readonly TheoryData<DbType, Type> DBToTypeData = new TheoryData<DbType, Type>
        {
            {DbType.Int16,typeof(short) },
            {DbType.Int64,typeof(long) },
            {DbType.Int32,typeof(int) },
            {DbType.Boolean,typeof(bool) },
            {DbType.String,typeof(string) },
            {DbType.DateTime2,typeof(DateTime) },
            {DbType.Decimal,typeof(decimal) },
            {DbType.Single,typeof(float) },
            {DbType.String,typeof(string) },
            {DbType.Time,typeof(TimeSpan) },
            {DbType.Binary,typeof(byte[]) },
        };

        public static readonly TheoryData<SqlDbType, DbType> SQLToDbTypeData = new TheoryData<SqlDbType, DbType>
        {
            {SqlDbType.SmallInt,DbType.Int16 },
            {SqlDbType.BigInt,DbType.Int64 },
            {SqlDbType.Int,DbType.Int32 },
            {SqlDbType.Bit,DbType.Boolean },
            {SqlDbType.NVarChar,DbType.String },
            {SqlDbType.DateTime2,DbType.DateTime2 },
            {SqlDbType.Decimal,DbType.Decimal },
            {SqlDbType.Real,DbType.Single },
            {SqlDbType.NVarChar,DbType.String },
            {SqlDbType.Time,DbType.Time },
            {SqlDbType.VarBinary,DbType.Binary },
        };

        [Fact]
        public void CanConvertTo()
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(typeof(DbType), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(SqlDbType)));
            Assert.True(Temp.CanConvertTo(typeof(Type)));
        }

        [Theory]
        [MemberData(nameof(DBToTypeData))]
        public void ConvertFrom(DbType sqlDbType, Type type)
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(sqlDbType, Temp.ConvertFrom(type));
        }

        [Theory]
        [MemberData(nameof(SQLToDbTypeData))]
        public void ConvertFrom2(SqlDbType sqlDbType, DbType dbType)
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(dbType, Temp.ConvertFrom(sqlDbType));
        }

        [Fact]
        public void ConvertFromUri()
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(DbType.String, Temp.ConvertFrom(typeof(Uri)));
        }

        [Theory]
        [MemberData(nameof(SQLToDbTypeData))]
        public void ConvertTo(SqlDbType sqlDbType, DbType dbType)
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(sqlDbType, Temp.ConvertTo(dbType, typeof(SqlDbType)));
        }

        [Theory]
        [MemberData(nameof(DBToTypeData))]
        public void ConvertTo2(DbType sqlDbType, Type type)
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(type, Temp.ConvertTo(sqlDbType, typeof(Type)));
        }
    }
}