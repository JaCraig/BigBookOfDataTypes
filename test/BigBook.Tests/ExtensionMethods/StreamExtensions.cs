using BigBook.Tests.BaseClasses;
using FileCurator;
using System.Threading.Tasks;
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
            using (var Test = File.OpenRead())
            {
                Assert.Equal("This is a test", Test.ReadAll());
            }
        }

        [Fact]
        public async Task ReadAllAsync()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (var Test = File.OpenRead())
            {
                Assert.Equal("This is a test", await Test.ReadAllAsync().ConfigureAwait(false));
            }
        }

        [Fact]
        public void ReadAllBinary()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (var Test = File.OpenRead())
            {
                var Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        [Fact]
        public void ReadAllBinary2()
        {
            using (var Test = new System.IO.MemoryStream())
            {
                Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
                var Content = Test.ReadAllBinary();
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        [Fact]
        public async Task ReadAllBinary2Async()
        {
            using (var Test = new System.IO.MemoryStream())
            {
                Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
                var Content = await Test.ReadAllBinaryAsync().ConfigureAwait(false);
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }

        [Fact]
        public async Task ReadAllBinaryAsync()
        {
            new FileInfo(@".\Testing\Test.txt").Write("This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using (var Test = File.OpenRead())
            {
                var Content = await Test.ReadAllBinaryAsync().ConfigureAwait(false);
                Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
            }
        }
    }
}