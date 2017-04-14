﻿using BigBook.Tests.BaseClasses;
using System;
using Xunit;

namespace BigBook.Tests
{
    public class DynamoTests : TestingDirectoryFixture
    {
        [Fact]
        public void CallMethod()
        {
            dynamic Temp = new TestClass
            {
                A = "Testing"
            };
            Temp.B = new Func<string>(() => Temp.A);
            Assert.Equal("Testing", Temp.B());
            Assert.Equal("Testing", Temp.A);
        }

        [Fact]
        public void ChangeLog()
        {
            dynamic Temp = new TestClass
            {
                A = "Testing"
            };
            Temp.B = new Func<string>(() => Temp.A);
            Assert.Equal(1, Temp.ChangeLog.Count);
            Assert.Contains("B", Temp.ChangeLog.Keys);
            dynamic Temp2 = new BigBook.Dynamo(new { A = "Testing" });
            Temp2.A = "Testing2";
            Assert.Equal("Testing", Temp2.ChangeLog["A"].OriginalValue);
            Assert.Equal("Testing2", Temp2.ChangeLog["A"].NewValue);
        }

        [Fact]
        public void ConvertToClass()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            TestClass2 Temp2 = Temp;
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);

            Temp = new BigBook.Dynamo();
            Temp.A = "Testing";
            Temp.B = 1;
            Temp2 = Temp;
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);

            Temp = new BigBook.Dynamo();
            Temp.A = "Testing";
            Temp.B = 1;
            Temp2 = Temp.To<TestClass2>();
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);
        }

        [Fact]
        public void Copy()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            dynamic Temp2 = new TestClass2();
            Temp2.A = "A";
            Temp.Copy(Temp2);
            Assert.Equal("A", Temp.A);
            Assert.Equal(0, Temp.B);
        }

        [Fact]
        public void CopyBetweenItems()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            BigBook.Dynamo Temp2 = Temp;
            Assert.Equal("Testing", Temp2["A"]);
            Assert.Equal(1, Temp2["B"]);

            Temp = new BigBook.Dynamo();
            Temp.A = "Testing2";
            Temp.B = 2;
            Temp2 = Temp;
            Assert.Equal("Testing2", Temp2["A"]);
            Assert.Equal(2, Temp2["B"]);
        }

        [Fact]
        public void CopyTo()
        {
            dynamic Temp = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            dynamic Temp2 = new TestClass2
            {
                A = "A"
            };
            Temp.CopyTo(Temp2);
            Assert.Equal("Testing", Temp2.A);
            Assert.Equal(1, Temp2.B);
        }

        [Fact]
        public void EqualValues()
        {
            dynamic Temp = new TestClass();
            dynamic Temp2 = new TestClass();
            Temp.A = "Testing";
            Temp.B = 1;
            Temp2.A = "Testing";
            Temp2.B = 1;
            Assert.True(Temp.Equals(Temp2));
        }

        [Fact]
        public void GetHashCodeTest()
        {
            dynamic Temp = new BigBook.Dynamo(new { A = "Testing", B = 1 });
            Temp.C = null;
            Temp.GetHashCode();
        }

        [Fact]
        public void Initialization()
        {
            dynamic Temp = new BigBook.Dynamo(new { A = "Testing", B = 1 });
            Assert.Equal("Testing", Temp.A);
            Assert.Equal(1, Temp.B);
        }

        [Fact]
        public void Keys()
        {
            dynamic Temp = new TestClass
            {
                A = "Testing"
            };
            Temp.B = new Func<string>(() => Temp.A);
            Assert.Equal(2, Temp.Keys.Count);
            Assert.Contains("A", Temp.Keys);
            Assert.Contains("B", Temp.Keys);
        }

        [Fact]
        public void SetAndGetMember()
        {
            dynamic Temp = new TestClass();
            Assert.Equal<string>((string)null, Temp.A);
            Temp.A = "Testing";
            Temp.B = 1;
            int B = Temp.B;
            Assert.Equal<string>("Testing", Temp.A);
            Assert.Equal<int>(1, B);
            Assert.Equal<string>("TestClass this\r\n\tSystem.String A = Testing\r\n\tSystem.Int32 B = 1\r\n", Temp.ToString());
            Temp.C = new Func<int>(() => 1);
            Assert.Equal<string>("TestClass this\r\n\tSystem.String A = Testing\r\n\tSystem.Int32 B = 1\r\n\tSystem.Func<System.Int32> C = System.Func`1[System.Int32]\r\n", Temp.ToString());
            Assert.Equal<int>(1, Temp.C());
        }

        [Fact]
        public void SubSet()
        {
            dynamic Temp = new BigBook.Dynamo(new { A = "Testing", B = 1 });
            Assert.Equal("Testing", Temp.A);
            Assert.Equal(1, Temp.B);
            Temp = Temp.SubSet("A");
            Assert.Equal("Testing", Temp.A);
            Assert.False(Temp.ContainsKey("B"));
            Temp.Remove("A");
            Assert.False(Temp.ContainsKey("A"));
        }

        [Fact]
        public void Values()
        {
            dynamic Temp = new TestClass
            {
                A = "Testing"
            };
            Temp.B = new Func<string>(() => Temp.A);
            Assert.Equal(2, Temp.Values.Count);
            Assert.Contains("Testing", Temp.Values);
        }

        public class TestClass : BigBook.Dynamo<TestClass>
        {
            public string A { get; set; }
        }

        public class TestClass2
        {
            public string A { get; set; }

            public int B { get; set; }
        }
    }
}