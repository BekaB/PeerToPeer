using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entites
{
    public class File
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FileHash { get; set; } 

        public System.Guid PeerID { get; set; } // identifies the peer that owns this file
        public string PeerName { get; set; } // classifier, or peer name does not guarantee uniqueness, used to reslove the peer
    }
}
