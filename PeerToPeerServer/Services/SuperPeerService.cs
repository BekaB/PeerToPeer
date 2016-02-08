using ClassLibrary.Entites;
using ClassLibrary.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PeerToPeerServer.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SuperPeerService" in both code and config file together.
    public class SuperPeerService : ISuperPeerService
    {
        private DBConnection.DBConnection dbConnection;
        private PeerToPeerServer.Utility.Log logging; 

        public SuperPeerService()
        {
            dbConnection = DBConnection.DBConnection.GetInstance();
            logging = PeerToPeerServer.Utility.Log.GetInstance();
        }

        public void AddFile(List<File> fileList)
        {
            foreach (var file in fileList)
            {
                string query = String.Format("INSERT INTO files (fileName, fileSize, fileType, fileHash, peerId, peerName)"+
                    " VALUES ('{0}', {1}, '{2}', '{3}', '{4}', '{5}')",
                    file.FileName, file.FileSize, file.FileType, file.FileHash, file.PeerID, file.PeerName);
                dbConnection.Update(query);

                // log the event
                logging.LogI(String.Format("Info: *** A new file '{0}' registered ***" +
                    Environment.NewLine, file.FileName));
            }
        }

        public void DeleteFile(File file)
        {
            string temp = String.Format("DELETE FROM files WHERE peerId='{0}' and fileHash='{1}' and fileName='{2}'",
                file.PeerID, file.FileHash, file.FileName);
            dbConnection.Update(temp);

            // log the event
            logging.LogI(String.Format("Info: *** A file '{0}' has been deleted ***" +
                Environment.NewLine, file.FileName));
        }

        public void AddPeer(Peer peer)
        {
            string temp = String.Format("INSERT INTO peers (peerId, peerName, peerHostName, comments) values ('{0}', '{1}', '{2}', '{3}')",
                peer.PeerID, peer.PeerName, peer.PeerHostName, peer.Comments);
            dbConnection.Update(temp);

            // log the event
            logging.LogI(String.Format("Info: *** A new peer '{0}' is connected ***" +
                Environment.NewLine, peer.PeerID.ToString()));
            logging.AddPeer(peer.PeerID.ToString());
        }

        public void RemovePeer(Peer peer) 
        {
            string temp = String.Format("DELETE FROM peers WHERE peerId='{0}'", peer.PeerID);
            dbConnection.Update(temp);

            // log the event
            logging.LogI(String.Format("Info: *** A peer '{0}' is disconnected ***" +
                Environment.NewLine, peer.PeerID));
            logging.RemovePeer(peer.PeerID.ToString());

            temp = String.Format("DELETE FROM files WHERE peerId='{0}'", peer.PeerID.ToString());
            int affectedRows = dbConnection.Update(temp);
            
            // log the event
            logging.LogI(String.Format("Info: *** {0} files are removed due to peer '{1}' disconnection ***" +
                Environment.NewLine, affectedRows, peer.PeerID));
        }

        public List<SearchFile> SearchAvailableFiles(string fileName)
        {
            string query = String.Format("SELECT * FROM files WHERE fileName LIKE '%{0}%'", fileName);
            SQLiteDataReader reader = dbConnection.Select(query);

            List<SearchFile> result = new List<SearchFile>();

            List<File> availableFiles = new List<File>();
            while (reader.Read())
            {
                File file = new File();

                file.FileName = (string)reader["fileName"];
                file.FileSize = long.Parse(reader["fileSize"].ToString());
                file.FileType = (string)reader["fileType"];
                file.FileHash = (string)reader["fileHash"];
                file.PeerName = (string)reader["peerName"];

                availableFiles.Add(file);
            }

            // create groups of the same files
            var sameFileGroups = availableFiles.GroupBy(x => x.FileHash); 

            foreach (var sameFiles in sameFileGroups)
            {
                // create groups of same files and with same name
                var sameNameAndFileGroups = sameFiles.GroupBy(x => x.FileName); 

                foreach (var temp in sameNameAndFileGroups) {
                    List<String> hosts = new List<String>();
                    SearchFile searchFile = new SearchFile();

                    File file = temp.First();

                    searchFile.FileName = file.FileName;
                    searchFile.FileSize = file.FileSize;
                    searchFile.FileType = file.FileType;
                    searchFile.FileHash = file.FileHash;

                    for (int i = 0; i < temp.Count(); i++)
                    {
                        hosts.Add(temp.ElementAt(i).PeerName);
                    }

                    searchFile.PeerNames = hosts;
                    result.Add(searchFile);
                }
            }

            return result;
        }
    }
}
