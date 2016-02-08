using ClassLibrary.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Entities
{

    // Used for display only
    public class DownloadFileDisplay : SearchFile
    { 
        private SearchFile parent;

        public DownloadFileDisplay(SearchFile parent) 
        {
            base.FileName = parent.FileName;
            base.FileSize = parent.FileSize;
            base.FileType = parent.FileType;
            base.FileHash = parent.FileHash;
            this.Seeders = parent.PeerNames.Count;

            this.parent = parent;
        }

        public int Seeders { get; set; }
        public double Perecent { get; set; }

        public SearchFile GetParent()
        {
            return this.parent;
        }
    }

    // Used for display only
    public class SearchFileDisplay : SearchFile
    {
        private SearchFile parent;

        public SearchFileDisplay() { }

        public SearchFileDisplay(SearchFile parent) 
        {
            base.FileName = parent.FileName;
            base.FileSize = parent.FileSize;
            base.FileType = parent.FileType;
            base.FileHash = parent.FileHash;
            this.Seeders = parent.PeerNames.Count;

            this.parent = parent;
        }

        public int Seeders { get; set; }

        public SearchFile GetParent() {
            return this.parent;
        }
    }
}
