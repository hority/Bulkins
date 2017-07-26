using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins
{
    /// <summary>
    /// Configuration for Bulkins
    /// </summary>
    public class BulkInsertOperation
    {
        public string DestinationTableName { get; set; }

        public FileInfo SourceFileInfo { get; set; } = new FileInfo(null);

        public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; } = SqlBulkCopyOptions.Default;

        /// <summary>
        /// Set SqlBulkCopyOptions
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        /// <remarks>This method OVERWRITES SqlBulkCopyOptions</remarks>
        public BulkInsertOperation Options(SqlBulkCopyOptions options)
        {
            this.SqlBulkCopyOptions = options;
            return this;
        }

        /// <summary>
        /// Specifies destination table for SqlBulkCopy 
        /// </summary>
        /// <param name="destinationTableName"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        public BulkInsertOperation To(string destinationTableName)
        {
            this.DestinationTableName = destinationTableName;
            return this;
        }

        /// <summary>
        /// Specifies path to source file for SqlBulkCopy 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        public BulkInsertOperation From(string sourceFilePath)
        {
            this.SourceFileInfo = new FileInfo(sourceFilePath);
            return this;
        }

        /// <summary>
        /// Specifies source file for SqlBulkCopy 
        /// </summary>
        /// <param name="sourceFileInfo"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        public BulkInsertOperation From(FileInfo sourceFileInfo)
        {
            this.SourceFileInfo = sourceFileInfo;
            return this;
        }
    }
}
