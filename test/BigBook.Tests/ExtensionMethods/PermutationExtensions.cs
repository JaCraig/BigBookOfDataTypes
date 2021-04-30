using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    /// <summary>
    /// Permutation extensions
    /// </summary>
    public class PermutationExtensionsTests : TestBaseClass
    {
        protected override System.Type ObjectType => typeof(PermutationExtensions);

        [Fact]
        public void BasicTest()
        {
            var TestObject = new System.Collections.Generic.List<string>();
            TestObject.AddRange(new string[] { "this", "is", "a", "test" });
            var Results = TestObject.Permute();
            Assert.Equal(24, Results.Keys.Count);
            foreach (var Key in Results.Keys)
            {
                foreach (var Item in Results[Key])
                {
                    Assert.True(Item == "this" || Item == "is" || Item == "a" || Item == "test");
                }
            }
        }
    }
}