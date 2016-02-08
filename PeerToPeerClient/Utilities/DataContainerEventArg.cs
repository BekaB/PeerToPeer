using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Utilities
{
    public sealed class DataContainerEventArg<T> : EventArgs
    {
        public DataContainerEventArg(T data) : base() { this.Data = data; }
        public T Data { get; set; }
    }
}
