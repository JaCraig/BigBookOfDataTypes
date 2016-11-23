using System;
using System.Data;
using Xunit;

namespace BigBook.Tests.Conversion.Converter
{
    public class DbTypeTypeConverterTests
    {
        [Fact]
        public void CanConvertTo()
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(typeof(DbType), Temp.AssociatedType);
            Assert.True(Temp.CanConvertTo(typeof(SqlDbType)));
            Assert.True(Temp.CanConvertTo(typeof(Type)));
        }

        [Fact]
        public void ConvertFrom()
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(DbType.Int16, Temp.ConvertFrom(SqlDbType.SmallInt));
            Assert.Equal(DbType.Int32, Temp.ConvertFrom(SqlDbType.Int));
            Assert.Equal(DbType.Int16, Temp.ConvertFrom(typeof(Int16)));
            Assert.Equal(DbType.Int32, Temp.ConvertFrom(typeof(Int32)));
        }

        [Fact]
        public void ConvertTo()
        {
            var Temp = new BigBook.Conversion.DbTypeTypeConverter();
            Assert.Equal(SqlDbType.SmallInt, Temp.ConvertTo(DbType.Int16, typeof(SqlDbType)));
            Assert.Equal(SqlDbType.Int, Temp.ConvertTo(DbType.Int32, typeof(SqlDbType)));
            Assert.Equal(typeof(int), Temp.ConvertTo(DbType.Int32, typeof(Type)));
            Assert.Equal(typeof(short), Temp.ConvertTo(DbType.Int16, typeof(Type)));
        }
    }
}