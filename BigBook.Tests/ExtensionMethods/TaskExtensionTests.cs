using BigBook.Tests.BaseClasses;
using System.Threading.Tasks;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class TaskExtensionTests : TestBaseClass
    {
        protected override System.Type ObjectType { get; set; } = typeof(TaskExtensions);

        [Fact]
        public void RunSync()
        {
            Assert.Equal(2, AsyncHelper.RunSync(() => MethodAsync(1)));
        }

        private Task<int> MethodAsync(int value)
        {
            return Task.FromResult(value + 1);
        }
    }
}