using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechTalk.SpecFlow;

namespace Automation.Project.Utilities
{
    class TableHandling
    {
        // Converts a gherkin table to a data table for querying data inside of the table
        public static DataTable ConvertGherkinTableToDataTable(Table gherkinTableName)
        {
            // Creates a new data table
            DataTable newDataTable = new DataTable();

            // For each header (column title) in the gherkin table, populate the datatable with the same exact header name
            foreach (var Header in gherkinTableName.Header)
            {
                newDataTable.Columns.Add(Header);
            }

            // For each row in the gherkin table, populate the datatable with the same exact row values
            foreach (var Row in gherkinTableName.Rows)
            {
                var newRow = newDataTable.Rows.Add();
                foreach (var Header in gherkinTableName.Header)
                {
                    newRow[Header] = Row[Header];
                }
            }

            return newDataTable;
        }

        // Return a list of a column's row values for a specified data table and column name
        public static List<string> getDataTableColumnValues(DataTable dataTableName, string columnName)
        {
            var rowValuesForSpecifiedColumn = dataTableName.AsEnumerable().Select(row => row.Field<string>($"{columnName}")).ToList();

            return rowValuesForSpecifiedColumn;
        }
    }
}
