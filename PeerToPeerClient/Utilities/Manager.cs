using ClassLibrary.Entites;
using PeerToPeerClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Utilities
{
    public class Manager
    {
        private TransferEngine transferEngine;
        public event EventHandler<DataContainerEventArg<FilePartData>> FilePartDownloaded;

        public Manager() {
            transferEngine = new TransferEngine();
            Peer = PeerServiceHost.Peer;
        }

        public Peer Peer { get; private set; }

        public List<ClassLibrary.Entites.SearchFile> SearchFileByName(string fileName)
        {
            SuperPeerServiceClient superPeerServiceClient = new SuperPeerServiceClient();
            List<ClassLibrary.Entites.SearchFile> filesList = superPeerServiceClient.SearchAvailableFiles(fileName);

            return filesList;
        }

        public void RemovePeer()
        {
            PeerServiceHost.PNRPManager.Leave(Peer);
        }

        public void DeleteFile(ClassLibrary.Entites.File file)
        {
            SuperPeerServiceClient ssc = new SuperPeerServiceClient();
            ssc.DeleteFile(file);
        }

        public void AddFile(ClassLibrary.Entites.File file)
        {
            SuperPeerServiceClient ssc = new SuperPeerServiceClient();

            List<ClassLibrary.Entites.File> tempList =  new List<ClassLibrary.Entites.File>();
            tempList.Add(file);

            ssc.AddFiles(tempList);
        }

        public void Download(ClassLibrary.Entites.SearchFile fileSearchResult)
        {
            Action<object> downloadAction = new Action<object>(StartDownload);
            Task downloadActionTask = new Task(downloadAction, fileSearchResult);
            downloadActionTask.Start();
        }

        private void StartDownload(object state)
        {
            ClassLibrary.Entites.SearchFile file = state as SearchFile;
            Action<object> downloadAction = new Action<object>(DownloadFilePart);

            ClassLibrary.Entites.File temp = new File();

            temp.FileName = file.FileName;
            temp.FileSize = file.FileSize;
            temp.FileType = file.FileType;
            temp.FileHash = file.FileHash;
            
            long partCount = file.FileSize / ClassLibrary.Config.FiePartsize;
            long mod = file.FileSize % ClassLibrary.Config.FiePartsize;

            if (mod > 0) partCount++;
            
            int allowedThreads = partCount < (long)ClassLibrary.Config.NumberThreads ? (int)partCount: 
                ClassLibrary.Config.NumberThreads;
            allowedThreads = allowedThreads < file.PeerNames.Count() ? allowedThreads :
                file.PeerNames.Count();
            
            int numIterations = (int)(partCount / allowedThreads);
            long part = 0;

            while (numIterations > 0)
            {
                int i = allowedThreads; 
             
                while (i > 0) {
                    temp.PeerName = file.PeerNames.ToArray()[--i];

                    Task downloadActionTask = new Task(downloadAction, new DownloadParameter { 
                        File = temp,
                        Part = part, 
                        AllPartsCount = partCount 
                    });

                    downloadActionTask.Start();

                    part++;
                }
             
                numIterations--;
            }
            
            mod = partCount % allowedThreads;

            while (mod > 0) 
            {
                temp.PeerName = file.PeerNames.ToArray()[--mod];

                Task downloadActionTask = new Task(downloadAction, new DownloadParameter
                {
                    File = temp,
                    Part = part,
                    AllPartsCount = partCount
                });

                downloadActionTask.Start();

                part++;
            }
        }

        private void DownloadFilePart(object downloadParameter)
        {
            DownloadParameter parameter = downloadParameter as DownloadParameter;

            try
            {
                byte[] data = transferEngine.GetFile(parameter.File, parameter.Part);
                OnFilePartDownloaded(new FilePartData(parameter, data));
            }

            catch (Exception)
            {
                throw new Exception("Download Failed!");
            }
        }

        public sealed class DownloadParameter
        {
            public long AllPartsCount { get; set; }
            public long Part { get; set; }
            public ClassLibrary.Entites.File File { get; set; }
        }

        private void OnFilePartDownloaded(FilePartData filePartData)
        {
            if (FilePartDownloaded != null)
            {
                FilePartDownloaded(this, new DataContainerEventArg<FilePartData>(filePartData));
            }
        }

        public class FilePartData
        {
            internal FilePartData(DownloadParameter downloadParameter, byte[] data)
            {
                DownloadParameter = downloadParameter;
                FileBytes = data;
            }

            public DownloadParameter DownloadParameter { get; private set; }
            public byte[] FileBytes { get; private set; }
        }
    }
}