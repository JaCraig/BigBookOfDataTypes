using BigBook.Tests.BaseClasses;
using FileCurator;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class StreamExtensionsTests : TestingDirectoryFixture
    {
        public StreamExtensionsTests()
        {
            new DirectoryInfo(@".\Testing").Create();
        }

        [Fact]
        public void ReadAll()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (System.IO.FileStream Test = File.OpenRead())
            {
                Assert.Equal("This is a test", Test.ReadAll());
            }
        }

        [Fact]
        public void ReadAllBinary()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (System.IO.FileStream Test = File.OpenRead())
            {
                var Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        [Fact]
        public void ReadAllBinary2()
        {
            using (System.IO.MemoryStream Test = new System.IO.MemoryStream())
            {
                Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
                var Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
        }
    }
}