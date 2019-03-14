using System.Diagnostics;
using System.Linq;
using Xunit;

namespace BigBook.Tests
{
    public class ProcessExtensionsTests
    {
        [Fact]
        public void GetInformation()
        {
            var Value = Process.GetProcesses().Take(4).GetInformation();
            Assert.NotNull(Value);
        }
    }
}