using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GovUk.Education.ExploreEducationStatistics.Common.Extensions;

namespace GovUk.Education.ExploreEducationStatistics.Data.Processor.Utils
{
    public static class CsvUtil
    {
        public static T BuildType<T>(IReadOnlyList<string> line, List<string> headers, string column,
            Func<string, T> func)
        {
            var value = Value(line, headers, column);
            return value == null ? default(T) : func(value);
        }

        public static T BuildType<T>(IReadOnlyList<string> line, List<string> headers, IEnumerable<string> columns,
            Func<string[], T> func)
        {
            var values = Values(line, headers, columns);
            return values.All(value => value == null) ? default(T) : func(values);
        }

        public static string[] Values(IReadOnlyList<string> line, List<string> headers, IEnumerable<string> columns)
        {
            return columns.Select(c => Value(line, headers, c)).ToArray();
        }

        public static string Value(IReadOnlyList<string> line, List<string> headers, string column)
        {
            return headers.Contains(column) ? line[headers.FindIndex(h => h.Equals(column))].Trim().NullIfWhiteSpace() : null;
        }

        public static List<string> GetColumnValues(DataColumnCollection cols)
        {
            return cols.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
        }

        public static List<string> GetRowValues(DataRow row)
        {
            return row.ItemArray.Select(x => x.ToString()).ToList();
        }
    }
}