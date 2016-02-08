using PeerToPeerClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Utilities
{
    public class TransferEngine
    {

        public byte[] GetFile(ClassLibrary.Entites.File file, long partNumber)
        {
            PNRPManager manager = PeerServiceHost.PNRPManager;
            List<ClassLibrary.Entites.Peer> foundPeers = manager.ResolveByPeerName(file.PeerName);

            if (foundPeers.Count != 0) 
            {
                ClassLibrary.Entites.Peer peer = foundPeers.FirstOrDefault();

                EndpointAddress endpointAddress = new EndpointAddress(string.Format("net.tcp://{0}:{1}/PeerToPeerClient.Services/PeerService",
                    peer.PeerHostName, ClassLibrary.Config.LocalPort));

                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                PeerServiceClient client = new PeerServiceClient(tcpBinding, endpointAddress);

                byte[] data = null;

                try
                {
                    data = client.TransferFile(file, partNumber);
                }
                catch
                {
                    throw new Exception("Unreachable host '" + file.PeerName);
                }

                return data;
            }

            else
            {
                throw new Exception("Unable to resolve peer " +  file.PeerName);
            }
        }
    }
}
