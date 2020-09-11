using System.Threading.Tasks;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class TaskExtensionTests
    {
        [Fact]
        public void RunSync()
        {
            Assert.Equal(2, MethodAsync(1).RunSync());
        }

        private Task<int> MethodAsync(int value)
        {
            return Task.FromResult(value + 1);
        }
    }
}