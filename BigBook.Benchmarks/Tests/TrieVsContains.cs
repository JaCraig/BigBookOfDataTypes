using BenchmarkDotNet.Attributes;
using System;

namespace BigBook.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class TrieVsContains
    {
        private StringTrie Trie { get; set; }

        [Benchmark(Baseline = true)]
        public void Contains()
        {
            var ComparisonText = "INSERT INTO [TestDatabase].[dbo].[TestTable](StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue,TimeSpanValue) VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8)".ToUpper();
            var Result = (ComparisonText.Contains("INSERT", StringComparison.Ordinal)
                                            || ComparisonText.Contains("UPDATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DELETE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("CREATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("ALTER", StringComparison.Ordinal)
                                            || ComparisonText.Contains("INTO", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DROP", StringComparison.Ordinal));
            ComparisonText = "SELECT * FROM TestUsers WHERE UserID=1".ToUpper();
            Result = (ComparisonText.Contains("INSERT", StringComparison.Ordinal)
                                            || ComparisonText.Contains("UPDATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DELETE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("CREATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("ALTER", StringComparison.Ordinal)
                                            || ComparisonText.Contains("INTO", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DROP", StringComparison.Ordinal));
            ComparisonText = "UPDATE [TestDatabase].[dbo].[TestTable] SET StringValue1=@0".ToUpper();
            Result = (ComparisonText.Contains("INSERT", StringComparison.Ordinal)
                                            || ComparisonText.Contains("UPDATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DELETE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("CREATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("ALTER", StringComparison.Ordinal)
                                            || ComparisonText.Contains("INTO", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DROP", StringComparison.Ordinal));
            ComparisonText = "Create Database TestDatabase".ToUpper();
            Result = (ComparisonText.Contains("INSERT", StringComparison.Ordinal)
                                            || ComparisonText.Contains("UPDATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DELETE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("CREATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("ALTER", StringComparison.Ordinal)
                                            || ComparisonText.Contains("INTO", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DROP", StringComparison.Ordinal));
            ComparisonText = "Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER,TimeSpanValue TIME(7))".ToUpper();
            Result = (ComparisonText.Contains("INSERT", StringComparison.Ordinal)
                                            || ComparisonText.Contains("UPDATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DELETE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("CREATE", StringComparison.Ordinal)
                                            || ComparisonText.Contains("ALTER", StringComparison.Ordinal)
                                            || ComparisonText.Contains("INTO", StringComparison.Ordinal)
                                            || ComparisonText.Contains("DROP", StringComparison.Ordinal));
        }

        [GlobalSetup]
        public void Setup()
        {
            Trie = new StringTrie();
            Trie.Add("INSERT", "DELETE", "UPDATE", "CREATE", "ALTER", "INTO", "DROP")
                .Build();
        }

        [Benchmark]
        public void TrieTest()
        {
            var Result = Trie.FindAny("INSERT INTO [TestDatabase].[dbo].[TestTable](StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue,TimeSpanValue) VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8)".ToUpper());
            Result = Trie.FindAny("SELECT * FROM TestUsers WHERE UserID=1".ToUpper());
            Result = Trie.FindAny("UPDATE [TestDatabase].[dbo].[TestTable] SET StringValue1=@0".ToUpper());
            Result = Trie.FindAny("Create Database TestDatabase".ToUpper());
            Result = Trie.FindAny("Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER,TimeSpanValue TIME(7))".ToUpper());
        }
    }
}