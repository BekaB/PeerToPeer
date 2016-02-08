using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace PeerToPeerServer.DBConnection
{
    public class DBConnection
    {
        private SQLiteConnection m_dbConnection;
        private static DBConnection instance;
        private SQLiteCommand command;

        private DBConnection() {
            //getDatabaseFile();
            m_dbConnection = new SQLiteConnection(String.Format(@"Data Source={0};Version=3;",
                ClassLibrary.Config.DatabaseFile), true);
            m_dbConnection.Open();

            CreateDBTables();
        }

        public static readonly object sharedObject = new object();

        public static DBConnection GetInstance() {

            if (instance == null) {
                lock (sharedObject) {
                    if (instance == null)
                        instance = new DBConnection();
                }
            }

            return instance;
        }

        private void CreateDBTables() {

            string sql = "CREATE TABLE IF NOT EXISTS peers (peerId VARCHAR(255), peerName VARCHAR(255),"+
                " peerHostName TEXT, comments TEXT)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE IF NOT EXISTS files (fileName VARCHAR(255), fileSize INT, fileType VARCHAR(255),"+
                " fileHash VARCHAR(255), peerId VARCHAR(255), peerName VARCHAR(100))";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public int Update(string sql) {
            command = new SQLiteCommand(sql, m_dbConnection);
            return command.ExecuteNonQuery();
        }

        public SQLiteDataReader Select(string sql)
        {
            command = new SQLiteCommand(sql, m_dbConnection);
            return command.ExecuteReader();
        }

        public void Close() {
            m_dbConnection.Close();
        }

        private string getDatabaseFile() 
        {
            string fileName = ClassLibrary.Config.DatabaseFile;
            FileInfo fInfo = new FileInfo(fileName);

            if (!fInfo.Exists) {
                DirectoryInfo dInfo = new DirectoryInfo(ClassLibrary.Config.ServerFolder);

                if (!dInfo.Exists)
                    dInfo.Create();

                fInfo.Create();
            }

            return fileName;
        } 
    }
}
