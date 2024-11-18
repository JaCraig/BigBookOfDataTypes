using BigBook.ExtensionMethods;
using BigBook.Tests.BaseClasses;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class StreamExtensionsTests : TestBaseClass
    {
        public StreamExtensionsTests()
        {
            new DirectoryInfo(@".\Testing").Create();
        }

        protected override System.Type ObjectType { get; set; } = null;

        [Fact]
        public void ReadAll()
        {
            WriteToFile(@".\Testing\Test.txt", "This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using var Test = File.OpenRead();
            Assert.Equal("This is a test", Test.ReadAll());
        }

        [Fact]
        public async Task ReadAllAsync()
        {
            WriteToFile(@".\Testing\Test.txt", "This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using var Test = File.OpenRead();
            Assert.Equal("This is a test", await Test.ReadAllAsync());
        }

        [Fact]
        public void ReadAllBinary()
        {
            WriteToFile(@".\Testing\Test.txt", "This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using var Test = File.OpenRead();
            var Content = Test.ReadAllBinary();
            Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }

        [Fact]
        public void ReadAllBinary2()
        {
            using var Test = new System.IO.MemoryStream();
            Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
            var Content = Test.ReadAllBinary();
            Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }

        [Fact]
        public async Task ReadAllBinary2Async()
        {
            using var Test = new System.IO.MemoryStream();
            Test.Write("This is a test".ToByteArray(), 0, "This is a test".Length);
            var Content = await Test.ReadAllBinaryAsync();
            Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }

        [Fact]
        public async Task ReadAllBinaryAsync()
        {
            WriteToFile(@".\Testing\Test.txt", "This is a test");
            var File = new System.IO.FileInfo(@".\Testing\Test.txt");
            using var Test = File.OpenRead();
            var Content = await Test.ReadAllBinaryAsync();
            Assert.Equal("This is a test", System.Text.Encoding.ASCII.GetString(Content, 0, Content.Length));
        }
    }
}