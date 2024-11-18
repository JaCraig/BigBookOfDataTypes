using BigBook.Tests.BaseClasses;
using System;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ExceptionExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType { get; set; } = typeof(ExceptionExtensions);

        [Fact]
        public void BasicExceptionOutput() => Assert.Equal($"Exception occurred{Environment.NewLine}Exception: Index was outside the bounds of the array.{Environment.NewLine}Exception Type: System.IndexOutOfRangeException{Environment.NewLine}StackTrace: {Environment.NewLine}Source: {Environment.NewLine}{Environment.NewLine}", new IndexOutOfRangeException().ToString("Exception occurred"));
    }
}