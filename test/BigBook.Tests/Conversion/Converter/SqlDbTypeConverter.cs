using System;
using System.Data;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class SqlDbTypeTypeConverterTests
    {
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

        public static readonly TheoryData<SqlDbType, Type> SQLToTypeData = new TheoryData<SqlDbType, Type>
        {
            {SqlDbType.SmallInt,typeof(short) },
            {SqlDbType.BigInt,typeof(long) },
            {SqlDbType.Int,typeof(int) },
            {SqlDbType.Bit,typeof(bool) },
            {SqlDbType.NChar,typeof(char) },
            {SqlDbType.DateTime2,typeof(DateTime) },
            {SqlDbType.Decimal,typeof(decimal) },
            {SqlDbType.Real,typeof(float) },
            {SqlDbType.NVarChar,typeof(string) },
            {SqlDbType.Time,typeof(TimeSpan) },
            {SqlDbType.VarBinary,typeof(byte[]) },
        };

        [Fact]
        public void CanConvertTo()

        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(typeof(SqlDbType), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(DbType)));
            Assert.True(Temp.CanConvertTo(typeof(Type)));
        }

        [Theory]
        [MemberData(nameof(SQLToTypeData))]
        public void ConvertFrom(SqlDbType sqlDbType, Type type)
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(sqlDbType, Temp.ConvertFrom(type));
        }

        [Theory]
        [MemberData(nameof(SQLToDbTypeData))]
        public void ConvertFrom2(SqlDbType sqlDbType, DbType dbType)
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(sqlDbType, Temp.ConvertFrom(dbType));
        }

        [Theory]
        [MemberData(nameof(SQLToDbTypeData))]
        public void ConvertTo(SqlDbType sqlDbType, DbType dbType)
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(dbType, Temp.ConvertTo(sqlDbType, typeof(DbType)));
        }

        [Theory]
        [MemberData(nameof(SQLToTypeData))]
        public void ConvertTo2(SqlDbType sqlDbType, Type type)
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(type, Temp.ConvertTo(sqlDbType, typeof(Type)));
        }
    }
}