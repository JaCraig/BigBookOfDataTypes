﻿using BigBook.Tests.BaseClasses;
using System;
using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class TrieTests : TestBaseClass<StringTrie>
    {
        public TrieTests()
        {
            TestObject = new StringTrie();
        }

        [Fact]
        public void FindAll()
        {
            const string TestString = "The quick brown fox jumps over the lazy dog";
            var TestObject = new StringTrie();
            TestObject.Add("jumps", "dog")
                .Build();

            var Results = TestObject.FindAll(TestString).ToArray();
            Assert.Equal(2, Results.Length);
            Assert.Equal("jumps", Results[0]);
            Assert.Equal("dog", Results[1]);
        }

        [Fact]
        public void FindAllSpan()
        {
            const string TestString = "The quick brown fox jumps over the lazy dog";
            var TestObject = new StringTrie();
            TestObject.Add("jumps", "dog")
                .Build();

            var Results = TestObject.FindAll(TestString.AsSpan()).ToArray();
            Assert.Equal(2, Results.Length);
            Assert.Equal("jumps", Results[0]);
            Assert.Equal("dog", Results[1]);
        }

        [Fact]
        public void FindAny()
        {
            const string TestString = "The quick brown fox jumps over the lazy dog";
            var TestObject = new StringTrie();
            TestObject.Add("jumps", "dog", "brown")
                .Build();

            var Results = TestObject.FindAny(TestString).ToArray();
            Assert.Equal("brown", new string(Results));
        }

        [Fact]
        public void FindLonger()
        {
            const string TestString = "The quick brown fox jumps over the lazy dog";
            var TestObject = new StringTrie();
            TestObject.Add("brown", "brown fox")
                .Build();

            var Results = TestObject.FindAny(TestString);
            var Results2 = TestObject.FindAll(TestString).ToArray();
            Assert.Equal("brown", Results);
            Assert.Equal("brown fox", Results2[1]);
            Assert.Equal("brown", Results2[0]);
        }
    }
}