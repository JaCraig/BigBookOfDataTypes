/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace BigBook
{
    /// <summary>
    /// Holds an individual row
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnNameHash">Column name hash</param>
        /// <param name="columnNames">Column names</param>
        /// <param name="columnValues">Column values</param>
        public Row(Hashtable columnNameHash, string[] columnNames, params object[] columnValues)
        {
            columnValues = columnValues ?? Array.Empty<object>();
            columnNames = columnNames ?? Array.Empty<string>();
            columnNameHash = columnNameHash ?? new Hashtable();
            ColumnNameHash = columnNameHash;
            ColumnNames = columnNames;
            ColumnValues = (object[])columnValues.Clone();
        }

        /// <summary>
        /// Column names
        /// </summary>
        public Hashtable ColumnNameHash { get; }

        /// <summary>
        /// Column names
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Column values
        /// </summary>
        public object[] ColumnValues { get; protected set; }

        /// <summary>
        /// Returns a column based on the column name specified
        /// </summary>
        /// <param name="columnName">Column name to search for</param>
        /// <returns>The value specified</returns>
        public object this[string columnName]
        {
            get
            {
                columnName = columnName ?? "";
                var Column = (int)ColumnNameHash[columnName];
                if (Column <= -1)
                {
                    return null;
                }

                return this[Column];
            }
        }

        /// <summary>
        /// Returns a column based on the value specified
        /// </summary>
        /// <param name="column">Column number</param>
        /// <returns>The value specified</returns>
        public object this[int column]
        {
            get
            {
                if (column < 0)
                {
                    column = 0;
                }

                if (ColumnValues.Length <= column)
                {
                    return null;
                }

                return ColumnValues[column];
            }
        }
    }

    /// <summary>
    /// Holds tabular information
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnNames">Column names</param>
        public Table(params string[] columnNames)
        {
            columnNames = columnNames ?? Array.Empty<string>();
            ColumnNames = (string[])columnNames.Clone();
            Rows = new List<Row>();
            ColumnNameHash = new Hashtable();
            int x = 0;
            for (int i = 0, columnNamesLength = columnNames.Length; i < columnNamesLength; i++)
            {
                string ColumnName = columnNames[i];
                if (!ColumnNameHash.ContainsKey(ColumnName))
                {
                    ColumnNameHash.Add(ColumnName, x++);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">Data reader to get the data from</param>
        public Table(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.FieldCount == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(reader), "reader.FieldCount needs to have at least 0 fields");
            }

            ColumnNames = new string[reader.FieldCount];
            for (int x = 0; x < reader.FieldCount; ++x)
            {
                ColumnNames[x] = reader.GetName(x);
            }
            ColumnNameHash = new Hashtable();
            int y = 0;
            for (int i = 0, ColumnNamesLength = ColumnNames.Length; i < ColumnNamesLength; i++)
            {
                string ColumnName = ColumnNames[i];
                if (!ColumnNameHash.ContainsKey(ColumnName))
                {
                    ColumnNameHash.Add(ColumnName, y++);
                }
            }

            Rows = new List<Row>();
            while (reader.Read())
            {
                object[] Values = new object[ColumnNames.Length];
                for (int x = 0; x < reader.FieldCount; ++x)
                {
                    Values[x] = reader[x];
                }
                Rows.Add(new Row(ColumnNameHash, ColumnNames, Values));
            }
        }

        /// <summary>
        /// Column Name hash table
        /// </summary>
        public Hashtable ColumnNameHash { get; }

        /// <summary>
        /// Column names for the table
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Rows within the table
        /// </summary>
        public List<Row> Rows { get; }

        /// <summary>
        /// Gets a specific row
        /// </summary>
        /// <param name="rowNumber">Row number</param>
        /// <returns>The row specified</returns>
        public Row this[int rowNumber]
        {
            get
            {
                if (Rows == null)
                {
                    return null;
                }

                return Rows.Count > rowNumber ? Rows[rowNumber] : null;
            }
        }

        /// <summary>
        /// Adds a row using the objects passed in
        /// </summary>
        /// <param name="objects">Objects to create the row from</param>
        /// <returns>This</returns>
        public Table AddRow(params object[] objects)
        {
            if (objects == null)
            {
                return this;
            }

            if (Rows == null)
            {
                return this;
            }

            Rows.Add(new Row(ColumnNameHash, ColumnNames, objects));
            return this;
        }
    }
}