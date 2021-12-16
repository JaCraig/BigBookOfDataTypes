using BigBook.Tests.BaseClasses;
using System.Text;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    /// <summary>
    /// StringBuilder extensions
    /// </summary>
    /// <seealso cref="TestBaseClass"/>
    public class StringBuilderExtensions : TestBaseClass
    {
        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        protected override System.Type ObjectType { get; set; } = null;

        [Fact]
        public void Trim()
        {
            Assert.Equal("Test", new StringBuilder("  Test  ").Trim().ToString());
            Assert.Equal("Test", new StringBuilder("  Test").Trim().ToString());
            Assert.Equal("Test", new StringBuilder("Test  ").Trim().ToString());
            Assert.Equal("Test", new StringBuilder("Test").Trim().ToString());
            Assert.Equal("", new StringBuilder().Trim().ToString());
            Assert.Equal(null, ((StringBuilder)null).Trim());
        }

        [Fact]
        public void TrimEnd()
        {
            Assert.Equal("  Test", new StringBuilder("  Test  ").TrimEnd().ToString());
            Assert.Equal("Test", new StringBuilder("Test").TrimEnd().ToString());
            Assert.Equal("", new StringBuilder().TrimEnd().ToString());
            Assert.Equal(null, ((StringBuilder)null).TrimEnd());
        }

        /// <summary>
        /// Trims the start.
        /// </summary>
        [Fact]
        public void TrimStart()
        {
            Assert.Equal("Test  ", new StringBuilder("  Test  ").TrimStart().ToString());
            Assert.Equal("Test", new StringBuilder("Test").TrimStart().ToString());
            Assert.Equal("", new StringBuilder().TrimStart().ToString());
            Assert.Equal(null, ((StringBuilder)null).TrimStart());
        }
    }
}