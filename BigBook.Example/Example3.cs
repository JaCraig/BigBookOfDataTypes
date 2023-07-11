namespace BigBook.Example
{
    /// <summary>
    /// Example 3. This example shows how to use the LazyAsync class.
    /// </summary>
    public static class Example3
    {
        /// <summary>
        /// Asynchronous lazy loading.
        /// </summary>
        public static async Task AsyncLazyLoading()
        {
            // This is an example of the lazy async class. It will only run the function when the value is requested.
            var TestObject = new LazyAsync<int>(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                return 5;
            });
            // This will take 500ms to run and will return 5.
            Console.WriteLine(await TestObject);
        }
    }
}