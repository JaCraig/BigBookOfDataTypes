using BigBook.Tests.BaseClasses;
using System;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ExceptionExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType { get; set; } = typeof(ExceptionExtensions);

        [Fact]
        public void BasicExceptionOutput() => Assert.Equal("Exception occurred\r\nException: Index was outside the bounds of the array.\r\nException Type: System.IndexOutOfRangeException\r\nStackTrace: \r\nSource: \r\n\r\n", new IndexOutOfRangeException().ToString("Exception occurred"));
    }
}