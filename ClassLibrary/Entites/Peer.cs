using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net;

namespace ClassLibrary.Entites
{
    public class Peer
    {
        public System.Guid PeerID { get; set; }
        // classifier, or peer name does not guarantee uniqueness, used to reslove the peer
        public string PeerName { get; set; }
        // name of the peer to peer Host, DNS encoded version of the PeerName
        public string PeerHostName { get; set; } 
        public string Comments { get; set; }
    }
}
