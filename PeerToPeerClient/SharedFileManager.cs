using ClassLibrary.Entites;
using Microsoft.VisualBasic.FileIO;
using PeerToPeerClient.Entities;
using PeerToPeerClient.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeerToPeerClient
{
    public partial class SharedFileManager : Form
    {
        private Manager manager;

        public SharedFileManager()
        {
            InitializeComponent();
            manager = new Manager();
            dataGridViewFManager.DataSource = LoadFiles();
        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            DialogResult result = openFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                string destination = ClassLibrary.Config.SharedFolder + "\\" + Path.GetFileName(openFileDialog.FileName);

                try
                {
                    FileSystem.CopyFile(openFileDialog.FileName, destination, UIOption.AllDialogs);

                    ClassLibrary.Entites.File file = new ClassLibrary.Entites.File();
                    FileInfo fileInfo = new FileInfo(destination);

                    file.FileName = fileInfo.Name;
                    file.FileType = fileInfo.Extension;
                    file.FileSize = fileInfo.Length;

                    SHA512 sha512 = SHA512.Create();
                    using (var stream = fileInfo.Open(FileMode.Open, FileAccess.Read))
                    {
                        file.FileHash = BitConverter.ToString(sha512.ComputeHash(stream), 0);
                    }

                    file.PeerName = manager.Peer.PeerName;
                    file.PeerID = manager.Peer.PeerID;

                    manager.AddFile(file);
                    dataGridViewFManager.DataSource = LoadFiles();
                }
                catch (OperationCanceledException) { }
            }
        }

        private List<SearchFile> LoadFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ClassLibrary.Config.SharedFolder);

            FileInfo[] filesInfo = dirInfo.GetFiles();
            List<SearchFile> files = new List<SearchFile>();

            foreach (var fileInfo in filesInfo)
            {
                ClassLibrary.Entites.File file = new ClassLibrary.Entites.File();
                file.FileName = fileInfo.Name;
                file.FileType = fileInfo.Extension;
                file.FileSize = fileInfo.Length;

                SHA512 sha512 = SHA512.Create();
                using (var stream = fileInfo.Open(FileMode.Open, FileAccess.Read))
                {
                    file.FileHash = BitConverter.ToString(sha512.ComputeHash(stream), 0);
                }

                file.PeerName = manager.Peer.PeerName;
                file.PeerID = manager.Peer.PeerID;

                files.Add(new SearchFile(file));
            }

            return files;
        }

        private void dataGridViewFManager_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SearchFile file = dataGridViewFManager.Rows[e.RowIndex].DataBoundItem as SearchFile;
            string filePath = ClassLibrary.Config.SharedFolder + @"\" + file.FileName;

            try
            {
                FileSystem.DeleteFile(filePath, UIOption.AllDialogs, RecycleOption.DeletePermanently);
                manager.DeleteFile(file.GetFile());
                dataGridViewFManager.DataSource = LoadFiles();
            }
            catch (OperationCanceledException) { }
        }
    }
}
