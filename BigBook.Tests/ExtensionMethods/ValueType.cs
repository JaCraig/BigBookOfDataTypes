using BigBook.ExtensionMethods;
using System.Text;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class ValueTypeTests
    {
        [Fact]
        public void Is()
        {
            Assert.True('a'.Is(CharIs.Lower));
            Assert.True('A'.Is(CharIs.Upper));
            Assert.True(' '.Is(CharIs.WhiteSpace));
        }

        [Fact]
        public void UnicodeTest()
        {
            const string Value = "\u25EF\u25EF\u25EF";
            Assert.True(Value.ToByteArray(new UnicodeEncoding()).IsUnicode());
        }
    }
}