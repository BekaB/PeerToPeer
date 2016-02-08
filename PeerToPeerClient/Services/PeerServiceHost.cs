using ClassLibrary;
using ClassLibrary.Entites;
using ClassLibrary.Services;
using PeerToPeerClient.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Services
{
    public class PeerServiceHost
    {
        private static PNRPManager pnrpManager;
        private static Peer peer;

        public void DoHost(Peer peer)
        {
            PeerServiceHost.peer = peer;

            string address = string.Empty;
            address = string.Format("net.tcp://{0}:{1}/PeerToPeerClient.Services/PeerService",
                peer.PeerHostName, Config.LocalPort);

            Uri uri = new Uri(address);
            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);

            ServiceHost serviceHost = new ServiceHost(typeof(PeerService), uri);
            serviceHost.AddServiceEndpoint(typeof(IPeerService), tcpBinding, "");

            serviceHost.Open();

            // add this peer to the server
            SuperPeerServiceClient ssc = new SuperPeerServiceClient();
            ssc.AddPeer(peer);

            // add the files that this peer wants to share to the server
            DirectoryInfo dirInfo = new DirectoryInfo(Config.SharedFolder);

            if (dirInfo.Exists)
            {
                FileInfo[] filesInfo = dirInfo.GetFiles();
                List<ClassLibrary.Entites.File> files = new List<ClassLibrary.Entites.File>();

                foreach (var fileInfo in filesInfo)
                {
                    ClassLibrary.Entites.File file = new ClassLibrary.Entites.File();
                    file.FileName = fileInfo.Name;
                    file.FileType = fileInfo.Extension;
                    file.FileSize = fileInfo.Length;

                    SHA512 sha512 = SHA512.Create(); 
                    using (var stream = fileInfo.Open(FileMode.Open, FileAccess.Read))
                    {
                        file.FileHash = BitConverter.ToString(sha512.ComputeHash(stream), 0);
                    }

                    file.PeerName = peer.PeerName;
                    file.PeerID = peer.PeerID;

                    files.Add(file);
                }

                if (files.Count != 0)
                    ssc.AddFiles(files);
            }

            else 
                dirInfo.Create();
        }

        public static PNRPManager PNRPManager { get { return pnrpManager; } }
        public static Peer Peer { get { return peer; } }

        public static void StartPeerServer() {
            pnrpManager = new PNRPManager(Config.LocalHostName, Config.LocalPort);
            PeerServiceHost host = new PeerServiceHost();
            host.DoHost(pnrpManager.Register());
        }
    }
}
