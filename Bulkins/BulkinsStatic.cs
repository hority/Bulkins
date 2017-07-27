using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

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
        public static void ExecuteBulkInsert(this SqlConnection connection, BulkInsertOperation config, SqlTransaction transaction = null)
        {
            using (var bcp = new SqlBulkCopy(connection, config.SqlBulkCopyOptions, transaction) { DestinationTableName = config.DestinationTableName })
            {
                var dt = ReadCSV(config.SourceFileInfo);
                bcp.WriteToServer(dt);
            }
        }

        /// <summary>
        /// Outputs results of SELECT statement into specified file with default options
        /// </summary>
        /// <param name="command"></param>
        /// <param name="filePath"></param>
        public static void ExportTo(this SqlCommand command,string filePath)
        {
            ExportTo(command, new FileInfo(filePath));
        }

        /// <summary>
        /// Outputs results of SELECT statement into specified file
        /// </summary>
        /// <param name="command"></param>
        /// <param name="fileInfo"></param>
        public static void ExportTo(this SqlCommand command, FileInfo fileInfo)
        {
            using (var dr = command.ExecuteReader())
            using (var sw = new StreamWriter(fileInfo.Path, false, Encoding.UTF8))
            {
                var c = dr.FieldCount;
                var sb = new StringBuilder();
                
                if (fileInfo.HasHeader)
                {
                    for (var i = 0; i < c; i++)
                    {
                        sb.Append(Value(dr.GetName(i))).Append(fileInfo.Delimiter);
                    }
                    if(sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sw.Write(sb.AppendLine().ToString());
                }

                while (dr.Read())
                {
                    sb.Clear();
                    for(var i = 0; i < c; i++)
                    {
                        sb.Append(Value(dr.GetValue(i))).Append(fileInfo.Delimiter);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sw.Write(sb.AppendLine().ToString());
                }

                sw.Flush();
            }
        }

        private static string Value(object fieldValue)
        {
            var ret = string.Empty;
            if (fieldValue != null)
            {
                if (fieldValue is string)
                {
                    ret = (string)fieldValue;
                }
                else
                {
                    ret = fieldValue.ToString();
                }
            }
            return $"\"{ret.Replace("\"", "\\\"")}\"";
        }

        /// <summary>
        /// Convert CSV to DataTable
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        private static DataTable ReadCSV(FileInfo fileInfo)
        {
            var dt = new DataTable();

            var parser = new TextFieldParser(fileInfo.Path);
            parser.SetDelimiters(new[] { fileInfo.Delimiter });
            parser.HasFieldsEnclosedInQuotes = true;

            if (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fileInfo.HasHeader)
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
