﻿using BigBook.Tests.BaseClasses;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class IDictionaryExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType { get; set; } = typeof(IDictionaryExtensions);

        [Fact]
        public void CopyToTest()
        {
            var Test = new Dictionary<string, int>();
            var Test2 = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Test.CopyTo(Test2);
            var Value = "";
            var Value2 = 0;
            foreach (var Key in Test2.Keys.OrderBy(x => x))
            {
                Value += Key;
                Value2 += Test2[Key];
            }
            Assert.Equal("ACQZ", Value);
            Assert.Equal(10, Value2);
        }

        [Fact]
        public void GetValue()
        {
            var Test = new Dictionary<string, int>
            {
                { "Q", 4 },
                { "Z", 2 },
                { "C", 3 },
                { "A", 1 }
            };
            Assert.Equal(4, Test.GetValue("Q"));
            Assert.Equal(0, Test.GetValue("V"));
            Assert.Equal(123, Test.GetValue("B", 123));
        }

        [Fact]
        public void SetValue()
        {
            var Test = new Dictionary<string, int>
            {
                { "Q", 4 },
                { "Z", 2 },
                { "C", 3 },
                { "A", 1 }
            };
            Assert.Equal(4, Test.GetValue("Q"));
            Test.SetValue("Q", 40);
            Assert.Equal(40, Test.GetValue("Q"));
        }

        [Fact]
        public void SortByValueTest()
        {
            IDictionary<string, int> Test = new Dictionary<string, int>
            {
                { "Q", 4 },
                { "Z", 2 },
                { "C", 3 },
                { "A", 1 }
            };
            Test = Test.Sort(x => x.Value);
            var Value = "";
            foreach (var Key in Test.Keys)
            {
                Value += Test[Key].ToString();
            }

            Assert.Equal("1234", Value);
        }

        [Fact]
        public void SortTest()
        {
            IDictionary<string, int> Test = new Dictionary<string, int>
            {
                { "Q", 4 },
                { "Z", 2 },
                { "C", 3 },
                { "A", 1 }
            };
            Test = Test.Sort();
            var Value = "";
            foreach (var Key in Test.Keys)
            {
                Value += Key;
            }

            Assert.Equal("ACQZ", Value);
        }
    }
}