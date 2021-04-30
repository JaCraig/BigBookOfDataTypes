using BigBook.Tests.BaseClasses;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class MatchCollectionExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType => typeof(MatchCollectionExtensions);

        [Fact]
        public void Where()
        {
            var Regex = new Regex(@"[^\s]");
            Assert.Equal(3, Regex.Matches("This is a test").Where(x => x.Value == "s").Count());
        }
    }
}