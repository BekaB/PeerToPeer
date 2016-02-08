using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Services;
using System.ServiceModel;
using PeerToPeerClient.Utilities;
using ClassLibrary;
using System.IO;

namespace PeerToPeerClient.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, 
        InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class PeerService:IPeerService
    {
        public byte[] TransferFile(ClassLibrary.Entites.File file, long partNumber)
        {
            string filePath = Config.SharedFolder + @"\" + file.FileName;
            return FileUtility.ReadFilePart(filePath, partNumber);
        }
    }
}
