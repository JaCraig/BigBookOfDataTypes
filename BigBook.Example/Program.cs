namespace BigBook.Example
{
    internal class ExampleClass
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Example1.StringExtensions();
            Example2.ListMappings();
            await Example3.AsyncLazyLoading();
        }
    }
}