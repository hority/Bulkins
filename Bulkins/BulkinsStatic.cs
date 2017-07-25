using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace Bulkins
{
    /// <summary>
    /// Core
    /// Implemented as an extension method for SqlConnection
    /// </summary>
    public static class BulkinsStatic
    {
        /// <summary>
        /// Exec Bulk Insert
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="conf"></param>
        /// <param name="options"></param>
        /// <param name="transaction"></param>
        public static void BulkInsert(this SqlConnection connection, Func<BulkInsertConfiguration, BulkInsertConfiguration> conf, SqlTransaction transaction = null)
        {
            var configuration = new BulkInsertConfiguration();
            if (conf != null)
            {
                configuration = conf(configuration);
            }

            using (var bcp = new SqlBulkCopy(connection, configuration.SqlBulkCopyOptions, transaction) { DestinationTableName = configuration.DestinationTableName })
            {
                var dt = ReadCSV(configuration.SourceFileInfo.Path, configuration.SourceFileInfo.HasHeader);
                bcp.WriteToServer(dt);
            }
        }

        /// <summary>
        /// Convert CSV to DataTable
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        private static DataTable ReadCSV(string sourceFilePath, bool hasHeader)
        {
            var dt = new DataTable();

            var parser = new TextFieldParser(sourceFilePath);
            parser.SetDelimiters(new[] { "," });
            parser.HasFieldsEnclosedInQuotes = true;

            if (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (hasHeader)
                {
                    for (var i = 0; i < fields.Length; i++)
                    {
                        dt.Columns.Add(new DataColumn(fields[i]));
                    }
                }
                else
                {
                    for (var i = 0; i < fields.Length; i++)
                    {
                        dt.Columns.Add(new DataColumn());
                    }

                    var row = dt.NewRow();
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        row[i] = fields[i];
                    }
                    dt.Rows.Add(row);
                }
            }

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                var row = dt.NewRow();
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    row[i] = fields[i];
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
