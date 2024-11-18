using BigBook.ExtensionMethods;

namespace BigBook.Example
{
    /// <summary>
    /// Example 1. This example shows how to use some of the string extensions.
    /// </summary>
    public static class Example1
    {
        /// <summary>
        /// String extension example.
        /// </summary>
        public static void StringExtensions()
        {
            // This is an example of one of the many extensions that are available. This one is used to keep only the characters you want.
            // In this case, it will only keep the letters.
            var ExampleString = "This is an example string #1".Keep(StringFilter.Alpha);

            Console.WriteLine(ExampleString);

            // In this case, it will only keep the letters and numbers.
            ExampleString = "This is an example string #2".Keep(StringFilter.Alpha | StringFilter.Numeric);

            Console.WriteLine(ExampleString);

            // You can also use regular expressions. This one will keep everything.
            ExampleString = "This is an example string #3".Keep(@"[\s\d\S]");

            Console.WriteLine(ExampleString);

            // There are a ton of extra extension methods available. This one will add spaces to the string by splitting on upper case letters.
            // This is useful for converting PascalCase to a sentence.
            ExampleString = nameof(Example1.StringExtensions).AddSpaces();

            Console.WriteLine(ExampleString);
        }
    }
}