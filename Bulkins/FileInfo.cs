using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins
{
    /// <summary>
    /// Source-file information for Bulkins
    /// </summary>
    public class FileInfo
    {
        public string Path { get; set; }
        public bool HasHeader { get; set; }
        public string Delimiter { get; set; }

        public FileInfo(string path, bool hasHeader = true, string delimiter = ",")
        {
            this.Path = path;
            this.HasHeader = hasHeader;
            this.Delimiter = delimiter;
        }
    }
}
