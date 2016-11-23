using System.Diagnostics;
using Xunit;

namespace BigBook.Tests
{
    public class ProcessExtensionsTests
    {
        [Fact]
        public void GetInformation()
        {
            var Value = Process.GetProcesses().GetInformation();
            Assert.NotNull(Value);
        }
    }
}