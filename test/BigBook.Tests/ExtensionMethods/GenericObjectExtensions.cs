using BigBook;
using BigBook.Tests.BaseClasses;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class GenericObjectExtensionsTests : TestingDirectoryFixture
    {
        [Fact]
        public void Execute1()
        {
            Func<int> Temp = () => 1;
            Temp.Execute();
        }

        [Fact]
        public void Execute2()
        {
            Action Temp = Test;
            Assert.Throws<Exception>(() => Temp.Execute());
        }

        [Fact]
        public void If()
        {
            var Temp = new MyTestClass();
            Assert.Same(Temp, Temp.Check(x => x.B == 10));
            Assert.NotSame(Temp, Temp.Check(x => x.B == 1));
        }

        [Fact]
        public void IsDefault()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
        }

        [Fact]
        public void IsNull()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
        }

        [Fact]
        public void IsNullOrDBNull()
        {
            Assert.False(new DateTime(1999, 1, 1).Is(default(DateTime)));
            object TestObject = null;
            Assert.True(TestObject.Is(default(object)));
            Assert.True(DBNull.Value.Is(DBNull.Value));
        }

        [Fact]
        public void NotIf()
        {
            var Temp = new MyTestClass();
            Assert.NotSame(Temp, Temp.Check(x => x.B != 10));
            Assert.Same(Temp, Temp.Check(x => x.B != 1));
        }

        [Fact]
        public void NullCheck()
        {
            object TestObject = new DateTime(1999, 1, 1);
            Assert.Equal(TestObject, TestObject.Check());
            Assert.Same(TestObject, TestObject.Check());
            TestObject = null;
            Assert.Equal(new DateTime(1999, 1, 2), TestObject.Check(new DateTime(1999, 1, 2)));
            Assert.Equal(new DateTime(1999, 1, 2), TestObject.Check(x => x != null, new DateTime(1999, 1, 2)));
        }

        [Fact]
        public void ThrowIf()
        {
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIf(x => x == null, new ArgumentNullException("TempName")));
            TempObject.ThrowIf(x => x != null, new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfDefault()
        {
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault("TempName"));
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfFalse()
        {
            Assert.Throws<Exception>(() => "ASDF".ThrowIfNot(x => string.IsNullOrEmpty(x), new Exception()));
            "ASDF".ThrowIfNot(x => !string.IsNullOrEmpty(x), new Exception());
        }

        [Fact]
        public void ThrowIfNotDefault()
        {
            default(DateTime).ThrowIfNotDefault("TempName");
            default(DateTime).ThrowIfNotDefault(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNull()
        {
            object TempObject = null;
            TempObject.ThrowIfNotNull("TempName");
            TempObject.ThrowIfNotNull(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNullOrDBNull()
        {
            DBNull.Value.ThrowIfNotNull("TempName");
            object TempObject = null;
            TempObject.ThrowIfNotNull("TempName");
            DBNull.Value.ThrowIfNotNull(new ArgumentNullException("TempName"));
            TempObject.ThrowIfNotNull(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNotNullOrEmpty()
        {
            const string TempObject = "";
            TempObject.ThrowIfNotNullOrEmpty("TempName");
            TempObject.ThrowIfNotNullOrEmpty(new ArgumentNullException("TempName"));
        }

        [Fact]
        public void ThrowIfNull()
        {
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrDBNull()
        {
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull("TempName"));
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull(new ArgumentNullException("TempName")));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrEmpty()
        {
            const string TempObject = "";
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfTrue()
        {
            "ASDF".ThrowIf(x => string.IsNullOrEmpty(x), new Exception());
            Assert.Throws<Exception>(() => "ASDF".ThrowIf(x => !string.IsNullOrEmpty(x), new Exception()));
        }

        [Fact]
        public void Times()
        {
            Assert.Equal(new int[] { 0, 1, 2, 3, 4 }.ToList(), 5.Times(x => x));
            var Builder = new StringBuilder();
            5.Times(x => { Builder.Append(x); });
            Assert.Equal("01234", Builder.ToString());
        }

        [Fact]
        public void To()
        {
            Assert.Equal(new Uri("http://A"), "http://A".To(new Uri("http://B")));
            Assert.Equal("http://A", new Uri("http://A").To("http://B"));
        }

        private void Test() => throw new Exception();
    }
}