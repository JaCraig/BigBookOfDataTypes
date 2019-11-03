﻿using BigBook;
using BigBook.Registration;

namespace TestSize
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();
            for (var x = 0; x < 10000000; ++x)
            {
                dynamic Item = new Dynamo(new { A = "This is a test" });
                TestClass TestClass = Item;
            }
        }

        private interface ITestInterface : ITestInterface2
        {
        }

        private interface ITestInterface2
        {
            string A { get; set; }
        }

        private abstract class TestAbstract : ITestInterface
        {
            public abstract string A { get; set; }

            public string B { get; set; }
        }

        private class TestClass : TestAbstract
        {
            public override string A { get; set; }
        }
    }
}