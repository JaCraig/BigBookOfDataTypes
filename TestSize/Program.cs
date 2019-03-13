using BigBook;
using BigBook.Registration;

namespace TestSize
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Canister.Builder.CreateContainer(null).RegisterBigBookOfDataTypes().Build();

            for (int x = 0; x < 10000000; ++x)
            {
                dynamic Item = new Dynamo(new { A = "This is a test" });
                TestClass TestClass = Item;
            }
        }

        private class TestClass
        {
            public string A { get; set; }
        }
    }
}