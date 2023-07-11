namespace BigBook.Example
{
    /// <summary>
    /// Example 2. This example shows how to use the ListMapping class.
    /// </summary>
    public static class Example2
    {
        /// <summary>
        /// ListMapping example.
        /// </summary>
        public static void ListMappings()
        {
            // Now for one of the many data types that are available. This one lets you have lists of items that are grouped by a key.
            var BucketFilterExample = new ListMapping<string, ExampleClass>();
            BucketFilterExample.Add("Test", new ExampleClass { Name = "Test", Value = 5 });
            BucketFilterExample.Add("Test", new ExampleClass { Name = "Test", Value = 10 });
            BucketFilterExample.Add("Test2", new ExampleClass { Name = "Test2", Value = 15 });
            BucketFilterExample.Add("Test2", new ExampleClass { Name = "Test2", Value = 20 });
            BucketFilterExample.Add("Test3", new ExampleClass { Name = "Test3", Value = 25 });
            BucketFilterExample.Add("Test3", new ExampleClass { Name = "Test3", Value = 30 });

            // Now we can get the values back out and they will be grouped by the key.
            // This also uses the extension method that is available to convert the list to a joined string.
            Console.WriteLine("Test1: {0}", BucketFilterExample["Test"].ToString(x => x.Value.ToString(), ", "));
            Console.WriteLine("Test2: {0}", BucketFilterExample["Test2"].ToString(x => x.Value.ToString(), ", "));
            Console.WriteLine("Test3: {0}", BucketFilterExample["Test3"].ToString(x => x.Value.ToString(), ", "));
        }
    }
}