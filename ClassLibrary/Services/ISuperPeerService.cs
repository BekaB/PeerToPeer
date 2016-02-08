using ClassLibrary.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ClassLibrary.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISuperPeerService" in both code and config file together.
    [ServiceContract]
    public interface ISuperPeerService
    {
        [OperationContract]
        void AddFile(List<File> fileList);

        [OperationContract]
        void DeleteFile(File file);

        [OperationContract]
        void AddPeer(Peer peer);

        [OperationContract]
        void RemovePeer(Peer peer);

        [OperationContract]
        List<SearchFile> SearchAvailableFiles(String fileName);
    }
}
