using System;
using System.Data;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class SqlDbTypeTypeConverterTests
    {
        [Fact]
        public void CanConvertTo()
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(typeof(SqlDbType), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(DbType)));
            Assert.True(Temp.CanConvertTo(typeof(Type)));
        }

        [Fact]
        public void ConvertFrom()
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(SqlDbType.SmallInt, Temp.ConvertFrom(DbType.Int16));
            Assert.Equal(SqlDbType.Int, Temp.ConvertFrom(DbType.Int32));
            Assert.Equal(SqlDbType.SmallInt, Temp.ConvertFrom(typeof(Int16)));
            Assert.Equal(SqlDbType.Int, Temp.ConvertFrom(typeof(Int32)));
        }

        [Fact]
        public void ConvertTo()
        {
            var Temp = new BigBook.Conversion.SqlDbTypeTypeConverter();
            Assert.Equal(DbType.Int16, Temp.ConvertTo(SqlDbType.SmallInt, typeof(DbType)));
            Assert.Equal(DbType.Int32, Temp.ConvertTo(SqlDbType.Int, typeof(DbType)));
            Assert.Equal(typeof(int), Temp.ConvertTo(SqlDbType.Int, typeof(Type)));
            Assert.Equal(typeof(short), Temp.ConvertTo(SqlDbType.SmallInt, typeof(Type)));
        }
    }
}