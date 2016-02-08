using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class Config
    {
        public const int FiePartsize = 10240;
        public const int NumberThreads = 5; // for file download

        const string SharedFolderNameKey = "PeerToPeerFileSharingSharedFolderName";
        const string PeerToPeerClientNameKey = "PeerToPeerClientName";
        const string PeerToPeerClientPortKey = "PeerToPeerClientPort";

        public static string SharedFolder
        {
            get
            {
                var sharedFolderName = Registry.CurrentUser.GetValue(SharedFolderNameKey);
                string folder = string.Empty;

                if (sharedFolderName == null)
                {
                    folder = @"C:\PeerToPeerSharedFolder"; 
                    Registry.CurrentUser.SetValue(SharedFolderNameKey, folder);
                }
                else
                {
                    folder = sharedFolderName.ToString();
                }

                return folder;
            }
        }

        public static string DatabaseFile
        {
            get
            {
                return @"C:\PeerToPeerServerFolder\MyDatabase.db";
            }
        }

        public static string ServerFolder
        {
            get
            {
                return @"C:\PeerToPeerServerFolder"; ;
            }
        }

        public static string LocalHostName
        {
            get
            {
                var localHostName = Registry.CurrentUser.GetValue(PeerToPeerClientNameKey);
                string hostname = string.Empty;

                if (localHostName == null)
                {
                    hostname = "PeerToPeerClient" + Guid.NewGuid().ToString().Replace("-", "");
                    Registry.CurrentUser.SetValue(PeerToPeerClientNameKey, hostname);
                }
                else
                {
                    hostname = localHostName.ToString();
                }

                return hostname;
            }
        }

        public static int LocalPort
        {
            get
            {
                var localPort = Registry.CurrentUser.GetValue(PeerToPeerClientPortKey);
                int port = 20388;

                if (localPort == null)
                {
                    Registry.CurrentUser.SetValue(PeerToPeerClientPortKey, port);
                }

                return port;
            }
        }
    }
}
