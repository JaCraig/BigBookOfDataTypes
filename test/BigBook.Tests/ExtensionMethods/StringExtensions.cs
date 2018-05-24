using BigBook.Formatters;
using BigBook.Tests.BaseClasses;

using FileCurator;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BigBook.Tests.ExtensionMethods
{
    public class StringExtensionsTests : TestingDirectoryFixture
    {
        [Fact]
        public void AddSpaces()
        {
            const string Value = "TheBrownFoxIsAwesome.ButTheBlueFoxIsNot.2222";
            Assert.Equal("The Brown Fox Is Awesome. But The Blue Fox Is Not.2222", Value.AddSpaces());
            const string Value2 = "IBM is an acronym, but IBM doesn't stand for IBM";
            Assert.Equal("IBM is an acronym, but IBM doesn't stand for IBM", Value2.AddSpaces());
        }

        [Fact]
        public void AlphaCharactersOnly()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot", Value.Keep(StringFilter.Alpha));
        }

        [Fact]
        public void AlphaNumericOnly()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot2222", Value.Keep(StringFilter.Alpha | StringFilter.Numeric));
        }

        [Fact]
        public void AppendLineFormat()
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("This is test {0}", 1);
            Assert.Equal("This is test 1" + System.Environment.NewLine, Builder.ToString());
            Builder.Clear();
            Builder.AppendLineFormat("Test {0}", 2)
                .AppendLineFormat("And {0}", 3);
            Assert.Equal("Test 2" + System.Environment.NewLine + "And 3" + System.Environment.NewLine, Builder.ToString());
        }

        [Fact]
        public void Base64Test()
        {
            const string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToBase64String().FromBase64(new ASCIIEncoding()));
            Assert.Equal("QVNERg==", Value.ToBase64String());
        }

        [Fact]
        public void ByteArrayTest()
        {
            const string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToByteArray().ToString(null));
        }

        [Fact]
        public void Center()
        {
            Assert.Equal("****This is a test****", "This is a test".Center(22, "*"));
            Assert.Equal("abcaThis is a testabca", "This is a test".Center(22, "abc"));
        }

        [Fact]
        public void CSSMinify()
        {
            var FileContent = new FileInfo(@"..\..\..\..\..\Data\Web\RandomCSS.css").Read();
            var MinifiedFileContent = new FileInfo(@"..\..\..\..\..\Data\Web\RandomCSS.css").Read().Minify(MinificationType.CSS);
            Assert.NotEqual(FileContent.Length, MinifiedFileContent.Length);
            Assert.True(FileContent.Length > MinifiedFileContent.Length);
        }

        [Fact]
        public void FilterOutText()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("The brown  is awsome. But the blue  is not", Value.Remove("fox"));
        }

        [Fact]
        public void FormatString()
        {
            Assert.Equal("(555) 555-1010", "5555551010".ToString("(###) ###-####"));
            Assert.Equal("(555) 555-1010", string.Format(new GenericStringFormatter(), "{0:(###) ###-####}", "5555551010"));
        }

        [Fact]
        public void FormatString2()
        {
            Assert.Equal("<A>This is a test</A><B>10</B><C>1.5</C>", "<A>{A}</A><B>{B}</B><C>{C}</C>".ToString(new StringFormatClass()));
        }

        [Fact]
        public void FormatString3()
        {
            Assert.Equal("<A>This is a test</A><B>10</B><C>1.5</C>", "<A>{A}</A><B>{B}</B><C>{C}</C>".ToString(new KeyValuePair<string, string>("{A}", "This is a test"),
                new KeyValuePair<string, string>("{B}", "10"),
                new KeyValuePair<string, string>("{C}", "1.5")));
        }

        [Fact]
        public void HTMLMinify()
        {
            var FileContent = new FileInfo(@"..\..\..\..\..\Data\Web\HanselmanSite.html").Read();
            var MinifiedFileContent = new FileInfo(@"..\..\..\..\..\Data\Web\HanselmanSite.html").Read().Minify(MinificationType.HTML);
            Assert.NotEqual(FileContent.Length, MinifiedFileContent.Length);
            Assert.True(FileContent.Length > MinifiedFileContent.Length);
        }

        [Fact]
        public void IsCreditCard()
        {
            Assert.True("4408041234567893".Is(StringCompare.CreditCard));
        }

        [Fact]
        public void IsUnicode()
        {
            const string Value = "\u25EF\u25EF\u25EF";
            Assert.True(Value.Is(StringCompare.Unicode));
        }

        [Fact]
        public void JavaScriptMinify()
        {
            var FileContent = new FileInfo(@"..\..\..\..\..\Data\Web\RandomJS.js").Read();
            var MinifiedFileContent = new FileInfo(@"..\..\..\..\..\Data\Web\RandomJS.js").Read().Minify(MinificationType.JavaScript);
            Assert.NotEqual(FileContent.Length, MinifiedFileContent.Length);
            Assert.True(FileContent.Length > MinifiedFileContent.Length);
        }

        [Fact]
        public void KeepFilterText()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("foxfox", Value.Keep("fox"));
        }

        [Fact]
        public void LeftTest()
        {
            const string Value = "ASDF";
            Assert.Equal("AS", Value.Left(2));
            Assert.Equal("", Value.Left(-2));
        }

        [Fact]
        public void LevenshteinDistance()
        {
            Assert.Equal(0, "".LevenshteinDistance(""));
            Assert.Equal(5, "".LevenshteinDistance("Tests"));
            Assert.Equal(5, "Tests".LevenshteinDistance(""));
            Assert.Equal(1, "Test".LevenshteinDistance("Tests"));
            Assert.Equal(3, "Test".LevenshteinDistance("Testing"));
            Assert.Equal(1, "Rest".LevenshteinDistance("Test"));
        }

        [Fact]
        public void MaskLeft()
        {
            Assert.Equal("####551010", "5555551010".MaskLeft());
        }

        [Fact]
        public void MaskRight()
        {
            Assert.Equal("5555######", "5555551010".MaskRight());
        }

        [Fact]
        public void NumberTimesOccurs()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal(2, Value.NumberTimesOccurs("is"));
        }

        [Fact]
        public void NumericOnly()
        {
            const string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("2222", Value.Keep(StringFilter.Numeric));
        }

        [Fact]
        public void RegexFormat()
        {
            Assert.Equal("(555) 555-1010", "5555551010".ToString(@"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"));
        }

        [Fact]
        public void RemoveExtraSpaces()
        {
            Assert.Equal("This is a test.", "This  is      a test.".Replace(StringFilter.ExtraSpaces, " "));
        }

        [Fact]
        public void Reverse()
        {
            const string Value = " this is a test";
            Assert.Equal("tset a si siht ", Value.Reverse());
        }

        [Fact]
        public void RightTest()
        {
            const string Value = "ASDF";
            Assert.Equal("DF", Value.Right(2));
            Assert.Equal("", Value.Left(-2));
        }

        [Fact]
        public void StringEncodingTest()
        {
            const string Value = "ASDF";
            Assert.Equal("ASDF", Value.Encode());
            Assert.Equal("ASDF", Value.Encode(new ASCIIEncoding(), new UTF32Encoding()).Encode(new UTF32Encoding(), new ASCIIEncoding()));
        }

        [Fact]
        public void StripHTML()
        {
            var FileContent = new FileInfo(@"..\..\..\..\..\Data\Web\HanselmanSite.html").Read();
            var MinifiedFileContent = new FileInfo(@"..\..\..\..\..\Data\Web\HanselmanSite.html").Read().StripHTML();
            Assert.NotEqual(FileContent.Length, MinifiedFileContent.Length);
            Assert.True(FileContent.Length > MinifiedFileContent.Length);
        }

        [Fact]
        public void StripLeft()
        {
            Assert.Equal("1010", "5555551010".StripLeft("5432"));
        }

        [Fact]
        public void StripRight()
        {
            Assert.Equal("555555", "5555551010".StripRight("10"));
        }

        [Fact]
        public void ToFirstCharacterUppercase()
        {
            const string Value = " this is a test";
            Assert.Equal(" This is a test", Value.ToString(StringCase.FirstCharacterUpperCase));
        }

        [Fact]
        public void ToSentenceCapitalize()
        {
            const string Value = " this is a test. of the sytem.";
            Assert.Equal(" This is a test. Of the sytem.", Value.ToString(StringCase.SentenceCapitalize));
        }

        [Fact]
        public void ToTitleCase()
        {
            const string Value = " this is a test";
            Assert.Equal(" This is a Test", Value.ToString(StringCase.TitleCase));
        }

        private enum EnumValues
        {
            Value1,
            Value2
        }

        public class StringFormatClass
        {
            public StringFormatClass()
            {
                A = "This is a test";
                B = 10;
                C = 1.5f;
            }

            public string A { get; set; }

            public int B { get; set; }

            public float C { get; set; }
        }
    }
}