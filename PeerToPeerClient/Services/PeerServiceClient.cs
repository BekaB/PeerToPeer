using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ClassLibrary.Services;

namespace PeerToPeerClient.Services
{
    public class PeerServiceClient : ClientBase<IPeerService>
    {
        public PeerServiceClient():base() {}

        public PeerServiceClient(string endpointConfigurationName):
                base(endpointConfigurationName) {}

        public PeerServiceClient(string endpointConfigurationName, string remoteAddress):
            base(endpointConfigurationName, remoteAddress) {}

        public PeerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress): 
                base(endpointConfigurationName, remoteAddress) {}

        public PeerServiceClient(System.ServiceModel.Channels.Binding binding, 
            System.ServiceModel.EndpointAddress remoteAddress):base(binding, remoteAddress) {}

        public byte[] TransferFile(ClassLibrary.Entites.File file, long partNumber)
        {
            byte[] _bytes = base.Channel.TransferFile(file, partNumber);
            return _bytes;
        }
    }
}
