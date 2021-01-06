using BigBook;
using Microsoft.Extensions.DependencyInjection;

namespace TestSize
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new ServiceCollection().AddCanisterModules(configure => configure.RegisterBigBookOfDataTypes());
            var Factory = Canister.Builder.Bootstrapper.Resolve<DynamoFactory>();
            for (var x = 0; x < 100000; ++x)
            {
                dynamic Item = Factory.Create(new { A = "This is a test" });
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