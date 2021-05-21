using BigBook.Tests.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public enum MyEnumTest
    {
        Item1,
        Item2,
        Item3
    }

    public interface IMyTestClass
    {
    }

    public class MyTestClass : IMyTestClass
    {
        public MyTestClass()
        {
            B = 10;
        }

        public virtual MyTestClass A { get; set; }

        public virtual int B { get; set; }
    }

    public class MyTestClass2
    {
        public MyTestClass2()
        {
            B = 20;
        }

        public virtual int B { get; set; }
    }

    public class TypeConversionTests : TestBaseClass
    {
        protected override Type ObjectType { get; set; } = null;

        [Fact]
        public void DbTypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, DbType.Int32.To(SqlDbType.Int));
            Assert.Equal(SqlDbType.NVarChar, DbType.String.To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Real, DbType.Single.To(SqlDbType.Int));
        }

        [Fact]
        public void DbTypeToType()
        {
            Assert.Equal(typeof(int), DbType.Int32.To(typeof(int)));
            Assert.Equal(typeof(string), DbType.String.To(typeof(int)));
            Assert.Equal(typeof(float), DbType.Single.To(typeof(int)));
        }

        [Fact]
        public void FormatToString()
        {
            var TestObject = new DateTime(1999, 1, 1);
            Assert.Equal("January 1, 1999", TestObject.FormatToString("MMMM d, yyyy"));
        }

        [Fact]
        public void SqlDbTypeToDbType()
        {
            Assert.Equal(DbType.Int32, SqlDbType.Int.To(DbType.Int32));
            Assert.Equal(DbType.String, SqlDbType.NVarChar.To(DbType.Int32));
            Assert.Equal(DbType.Single, SqlDbType.Real.To(DbType.Int32));
        }

        [Fact]
        public void SqlDbTypeToType()
        {
            Assert.Equal(typeof(int), SqlDbType.Int.To(typeof(int)));
            Assert.Equal(typeof(string), SqlDbType.NVarChar.To(typeof(int)));
            Assert.Equal(typeof(float), SqlDbType.Real.To(typeof(int)));
        }

        [Fact]
        public void ToExpando()
        {
            var TestObject = new MyTestClass();
            var Object = TestObject.To<MyTestClass, ExpandoObject>();
            Assert.Equal(10, ((IDictionary<string, object>)Object)["B"]);
            ((IDictionary<string, object>)Object)["B"] = 20;
            Assert.Equal(20, Object.To(new MyTestClass()).B);
        }

        [Fact]
        public void TryConvert()
        {
            Assert.Equal(1, (1.0f).To(0));
            Assert.Equal("2011", (2011).To(""));
            Assert.NotNull(new MyTestClass().To<MyTestClass, IMyTestClass>());
            Assert.NotNull(((object)new MyTestClass()).To<object, IMyTestClass>());
            Assert.NotNull(new MyTestClass().To<MyTestClass, MyTestClass2>());
            Assert.NotNull(((object)new MyTestClass()).To<object, MyTestClass2>());
            var Result = new MyTestClass().To<MyTestClass, MyTestClass2>();
            Assert.Equal(10, Result.B);
        }

        [Fact]
        public void TypeToDbType()
        {
            Assert.Equal(DbType.Int32, typeof(int).To(DbType.Int32));
            Assert.Equal(DbType.String, typeof(string).To(DbType.Int32));
            Assert.Equal(DbType.Single, typeof(float).To(DbType.Int32));
            Assert.Equal(DbType.Int32, typeof(MyEnumTest).To(DbType.Int32));
        }

        [Fact]
        public void TypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, typeof(int).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.NVarChar, typeof(string).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Real, typeof(float).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Int, typeof(MyEnumTest).To(SqlDbType.Int));
        }
    }
}