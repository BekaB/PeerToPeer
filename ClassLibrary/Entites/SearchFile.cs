using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entites
{
    // search result files
    public class SearchFile
    {
        private File file;

        public SearchFile() {
            PeerNames = new List<string>();
        }

        public SearchFile(File file)
        {
            this.FileName = file.FileName;
            this.FileSize = file.FileSize;
            this.FileType = file.FileType;
            this.FileHash = file.FileHash;

            this.file = file;
        }

        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public virtual string FileHash { get; set; }

        public List<String> PeerNames { get; set; }

        public File GetFile()
        {
            return this.file;
        }
    }
}
