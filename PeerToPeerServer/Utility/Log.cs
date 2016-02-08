using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerServer.Utility
{

    public class Log
    {
        public event EventHandler<LogEventArg<String>> InfoChanged;
        public event EventHandler<LogEventArg<List<String>>> PeersChanged;

        private String logInfo;
        private List<String> connectedPeers = new List<string>();
        private static Log instance = null; 
        public static readonly object sharedObject = new object();

        private Log() { }

        public static Log GetInstance() 
        {
            if (instance == null)
            {
                lock (sharedObject) {
                    if (instance == null) 
                        instance = new Log();
                }
            }

            return instance;
        }

        private void OnInfoChanged(LogEventArg<String> e)
        {
            if (InfoChanged != null)
                InfoChanged(this, e);
        }

        private void OnPeersChanged(LogEventArg<List<String>> e)
        {
            if (InfoChanged != null)
                PeersChanged(this, e);
        }

        public void LogI(string info) {
            logInfo = info;
            OnInfoChanged(new LogEventArg<String>(info));
        }

        public void AddPeer(String peerIP) {
            connectedPeers.Add(peerIP);
            OnPeersChanged(new LogEventArg<List<string>>(connectedPeers));
        }

        public void RemovePeer(String peerIP)
        {
            connectedPeers.Remove(peerIP);
            OnPeersChanged(new LogEventArg<List<string>>(connectedPeers));
        }
    }

    public sealed class LogEventArg<T> : EventArgs
    {
        public LogEventArg(T data) : base() { this.Data = data; }
        public T Data { get; set; }
    }
}
