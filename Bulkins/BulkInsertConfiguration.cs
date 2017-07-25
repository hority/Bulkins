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
    public class BulkInsertConfiguration
    {
        public string DestinationTableName { get; set; }

        public SourceFileInfo SourceFileInfo { get; set; } = new SourceFileInfo(null);

        public SqlBulkCopyOptions SqlBulkCopyOptions { get; set; } = SqlBulkCopyOptions.Default;

        /// <summary>
        /// Set SqlBulkCopyOptions
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        /// <remarks>This method OVERWRITES SqlBulkCopyOptions</remarks>
        public BulkInsertConfiguration Options(SqlBulkCopyOptions options)
        {
            this.SqlBulkCopyOptions = options;
            return this;
        }

        /// <summary>
        /// Specifies destination table for SqlBulkCopy 
        /// </summary>
        /// <param name="destinationTableName"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        public BulkInsertConfiguration To(string destinationTableName)
        {
            this.DestinationTableName = destinationTableName;
            return this;
        }

        /// <summary>
        /// Specifies source file for SqlBulkCopy 
        /// </summary>
        /// <param name="sourceFileInfo"></param>
        /// <returns>Instance of BulkInsertConfiguration</returns>
        public BulkInsertConfiguration From(SourceFileInfo sourceFileInfo)
        {
            this.SourceFileInfo = sourceFileInfo;
            return this;
        }
    }
}
