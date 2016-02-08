using ClassLibrary.Entites;
using ClassLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Services
{
    public class SuperPeerServiceClient : ClientBase<ISuperPeerService>
    {
        public SuperPeerServiceClient() {
        }
        
        public SuperPeerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SuperPeerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SuperPeerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }

        public SuperPeerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void AddFiles(List<File> filesList) {
            base.Channel.AddFile(filesList);
        }

        public void DeleteFile(File file)
        {
            base.Channel.DeleteFile(file);
        }
        
        public void AddPeer(Peer Peer) {
            base.Channel.AddPeer(Peer);
        }

        public void RemovePeer(Peer Peer)
        {
            base.Channel.RemovePeer(Peer);
        }

        public List<SearchFile> SearchAvailableFiles(string fileName)
        {
            return base.Channel.SearchAvailableFiles(fileName);
        }
    }
}