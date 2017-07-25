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
    public class SourceFileInfo
    {
        public string Path { get; set; }
        public bool HasHeader { get; set; }

        public SourceFileInfo(string path, bool hasHeader = false)
        {
            this.Path = path;
            this.HasHeader = hasHeader;
        }
    }
}
