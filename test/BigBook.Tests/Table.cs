using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests
{
    public class TableTests : TestBaseClass<Table>
    {
        public TableTests()
        {
            TestObject = new Table("Column1", "Column2", "Column3");
        }

        [Fact]
        public void CreationTest()
        {
            var Table = new BigBook.Table("Column1", "Column2", "Column3");
            Assert.Equal(3, Table.ColumnNames.Length);
            Assert.Equal("Column1", Table.ColumnNames[0]);
            Assert.Equal("Column2", Table.ColumnNames[1]);
            Assert.Equal("Column3", Table.ColumnNames[2]);
        }

        [Fact]
        public void RowCreationTest()
        {
            var Table = new BigBook.Table("Column1", "Column2", "Column3");
            Table.AddRow(1, "A", 9.2f)
                 .AddRow(2, "B", 8.2f)
                 .AddRow(3, "C", 7.2f);
            Assert.Equal(3, Table.Rows.Count);
            Assert.Equal(1, Table[0][0]);
            Assert.Equal("A", Table[0][1]);
            Assert.Equal(9.2f, Table[0][2]);
            Assert.Equal(2, Table[1][0]);
            Assert.Equal("B", Table[1][1]);
            Assert.Equal(8.2f, Table[1][2]);
            Assert.Equal(3, Table[2][0]);
            Assert.Equal("C", Table[2][1]);
            Assert.Equal(7.2f, Table[2][2]);
            Assert.Equal("Column1", Table[0].ColumnNames[0]);
            Assert.Equal("Column2", Table[0].ColumnNames[1]);
            Assert.Equal("Column3", Table[0].ColumnNames[2]);
            Assert.Equal(1, Table[0]["Column1"]);
            Assert.Equal("A", Table[0]["Column2"]);
            Assert.Equal(9.2f, Table[0]["Column3"]);
            Assert.Equal(2, Table[1]["Column1"]);
            Assert.Equal("B", Table[1]["Column2"]);
            Assert.Equal(8.2f, Table[1]["Column3"]);
            Assert.Equal(3, Table[2]["Column1"]);
            Assert.Equal("C", Table[2]["Column2"]);
            Assert.Equal(7.2f, Table[2]["Column3"]);
        }
    }
}