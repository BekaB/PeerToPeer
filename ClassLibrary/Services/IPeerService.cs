using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ClassLibrary.Services
{
    [ServiceContract()]
    public interface IPeerService
    {
        [OperationContractAttribute(IsOneWay = false)]
        byte[] TransferFile(ClassLibrary.Entites.File file, long partNumber);
    }
}
