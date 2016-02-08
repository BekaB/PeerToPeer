using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ClassLibrary.Entites;
using System.Net.PeerToPeer;
using PeerToPeerClient.Services;
using System.Net;

namespace PeerToPeerClient.Utilities
{
    public class PNRPManager
    {
        private PeerNameRegistration registration;

        public PNRPManager(string classifierName, int port)
        {
            this.ClassifierName = classifierName;
            this.Port = port;

            PeerName peer = new PeerName(classifierName, PeerNameType.Unsecured);
            registration = new PeerNameRegistration(peer, port, Cloud.AllLinkLocal) { UseAutoEndPointSelection = true};
        }

        public string ClassifierName { get; private set; }
        public int Port { get; private set; }
        public PeerNameRegistration Registration { get { return registration; } }

        public Peer Register() 
        {
            string timeStamp = string.Format("PeerToPeerClient Created at : {0}", DateTime.Now.ToShortTimeString());
            registration.Comment = timeStamp;

            try
            {
                registration.Start();
                Peer peer = new Peer()
                {
                    PeerID = Guid.NewGuid(),
                    PeerName = registration.PeerName.Classifier,
                    PeerHostName = registration.PeerName.PeerHostName,
                    Comments = registration.Comment
                };

                return peer;
            }
            catch (PeerToPeerException PEX)
            {
                throw new Exception(PEX.InnerException.Message);
            }
        }

        public void Leave(Peer peer)
        {
            registration.Stop();
            SuperPeerServiceClient ssc = new SuperPeerServiceClient();
            ssc.RemovePeer(peer);
        }

        public List<Peer> ResolveByPeerName(string peerName)
        {
            try
            {
                if (string.IsNullOrEmpty(peerName))
                    throw new ArgumentException("Cannot have a null or empty peer name.");

                PeerNameResolver resolver = new PeerNameResolver();
                PeerNameRecordCollection resolvedName = resolver.Resolve(new PeerName(peerName, PeerNameType.Unsecured),
                    Cloud.AllLinkLocal);
                
                List<Peer> foundPeers = new List<Peer>();
                foreach (PeerNameRecord foundItem in resolvedName)
                {
                    Peer peer = new Peer(){
                        PeerName = foundItem.PeerName.Classifier,
                        PeerHostName = foundItem.PeerName.PeerHostName,
                        Comments = foundItem.Comment
                    };

                    foundPeers.Add(peer);
                }

                return foundPeers;
            }
            catch (PeerToPeerException px)
            {
                throw new Exception(px.InnerException.Message);
            }
        }     
    }
}
