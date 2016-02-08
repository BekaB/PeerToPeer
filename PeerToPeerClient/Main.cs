using ClassLibrary.Entites;
using PeerToPeerClient.Entities;
using PeerToPeerClient.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeerToPeerClient
{
    public partial class Main : Form
    {
        private Manager manager;
        private List<DownloadFileDisplay> downloadingFiles;
        private Boolean IsSearchResultDisplay = false;
        private List<List<Tuple<Manager.DownloadParameter, Byte[]>>> downloadingData;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            manager = new Manager();
            manager.FilePartDownloaded += manager_FilePartDownloaded;
            downloadingFiles = new List<DownloadFileDisplay>();
            downloadingData = new List<List<Tuple<Manager.DownloadParameter,byte[]>>>();
            dataGridViewMain.DataSource = downloadingFiles;
        }

        private void buttonSearchFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBoxFileName.Text))
            {
                labelDisplayTitle.Text = "Search Result:";
                IsSearchResultDisplay = true;

                List<SearchFile> files = manager.SearchFileByName(textBoxFileName.Text);
                List<SearchFileDisplay> tempFiles = new List<SearchFileDisplay>();

                foreach (SearchFile f in files) {
                    SearchFileDisplay temp = new SearchFileDisplay(f);
                    tempFiles.Add(temp);
                }

                dataGridViewMain.DataSource = tempFiles;
            }
        }

        private void buttonSharedFiles_Click(object sender, EventArgs e)
        {
            SharedFileManager sharedFileManager = new SharedFileManager();
            sharedFileManager.ShowDialog(this);
        }

        private void manager_FilePartDownloaded(object sender, DataContainerEventArg<Manager.FilePartData> e)
        {
            // find a previous list that contains downloading information of this file
            List<Tuple<Manager.DownloadParameter, Byte[]>> prevData = downloadingData.FirstOrDefault(d => 
                d.First().Item1.File.FileName == e.Data.DownloadParameter.File.FileName &&
                d.First().Item1.File.FileHash == e.Data.DownloadParameter.File.FileHash);

            if (prevData == null) 
            {
                // if no list before create a new one 
                prevData = new List<Tuple<Manager.DownloadParameter, Byte[]>>();
                prevData.Add(new Tuple<Manager.DownloadParameter, Byte[]>(e.Data.DownloadParameter, e.Data.FileBytes));
                downloadingData.Add(prevData);
            }
            else {
                downloadingData.FirstOrDefault(d => 
                d.First().Item1.File.FileName == e.Data.DownloadParameter.File.FileName &&
                d.First().Item1.File.FileHash == e.Data.DownloadParameter.File.FileHash).
                    Add(new Tuple<Manager.DownloadParameter, Byte[]>(e.Data.DownloadParameter, e.Data.FileBytes));
            }

            // find the number of downloaded parts by counting tuples in the list that contains downloading information of this file
            int numDownParts = downloadingData.FirstOrDefault(d =>
                d.First().Item1.File.FileName == e.Data.DownloadParameter.File.FileName &&
                d.First().Item1.File.FileHash == e.Data.DownloadParameter.File.FileHash).Count;
            
            // find the file that shows downloading information of this file
            DownloadFileDisplay file = downloadingFiles.FirstOrDefault(f =>
                f.FileName == e.Data.DownloadParameter.File.FileName && f.FileHash == e.Data.DownloadParameter.File.FileHash);

            // increment the percently
            file.Perecent += (numDownParts / e.Data.DownloadParameter.AllPartsCount) * 100;

            if (numDownParts == e.Data.DownloadParameter.AllPartsCount)
            {
                downloadingFiles.RemoveAll(f =>
                    f.FileName == e.Data.DownloadParameter.File.FileName && f.FileHash == e.Data.DownloadParameter.File.FileHash);
                SaveFile(e.Data.DownloadParameter);
            }

            dataGridViewMain.DataSource = downloadingFiles;
        }

        private void SaveFile(Manager.DownloadParameter parameter)
        {
            CheckForIllegalCrossThreadCalls = false;

            List<Tuple<Manager.DownloadParameter, Byte[]>> prevData = downloadingData.FirstOrDefault(d =>
                d.First().Item1.File.FileName == parameter.File.FileName &&
                d.First().Item1.File.FileHash == parameter.File.FileHash);

            downloadingData.RemoveAll(d =>
                d.First().Item1.File.FileName == parameter.File.FileName &&
                d.First().Item1.File.FileHash == parameter.File.FileHash);

            var lst = prevData.OrderBy(x => x.Item1.Part).ToList();
            var bytes = new List<byte>();

            for (int i = 0; i < lst.Count; i++)
            {
                bytes.AddRange(lst[i].Item2);
            }

            string filePath = ClassLibrary.Config.SharedFolder + @"\" + parameter.File.FileName;

            System.IO.File.WriteAllBytes(filePath, bytes.ToArray());

            MessageBox.Show(this, String.Format("The download of file '{0}' is completed!", parameter.File.FileName),
                "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            manager.RemovePeer();
        }

        private void dataGridViewMain_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (IsSearchResultDisplay)
            {
                labelDisplayTitle.Text = "Downloading files:";
                textBoxFileName.Text = "";
                IsSearchResultDisplay = false;

                SearchFileDisplay file = dataGridViewMain.Rows[e.RowIndex].DataBoundItem as SearchFileDisplay;
                manager.Download(file.GetParent());

                DownloadFileDisplay temp = new DownloadFileDisplay(file.GetParent());

                downloadingFiles.Add(temp);
                dataGridViewMain.DataSource = downloadingFiles;
            }
        }
    }
}
