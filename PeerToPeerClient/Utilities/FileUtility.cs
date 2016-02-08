using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerClient.Utilities
{
    public class FileUtility
    {
        private static object sharedObject = new object();

        public static string FindFileByHash(string path,string fileName ,string hash)
        {
            var files = Directory.GetFiles(Config.SharedFolder, fileName, SearchOption.AllDirectories);

            foreach (var item in files)
            {
                SHA512 sha512 = SHA512.Create();
                using (FileStream fstream = new FileStream(item, FileMode.Open, FileAccess.Read))
                {
                    var computedhash = sha512.ComputeHash(fstream);                    
                    if (string.Compare(BitConverter.ToString(computedhash, 0), hash, true) == 0)
                    {
                        return item;
                    }
                }    
            }

            return null;
        }

        public static byte[] ReadFilePart(string filePath, long partNumber)
        {
            try
            {
                lock (sharedObject)
                {
                    using (FileStream fstream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        long remData = fstream.Length - (partNumber * Config.FiePartsize);
                        int arraySize = remData < Config.FiePartsize ? (int)remData : Config.FiePartsize;

                        byte[] data = new byte[arraySize];

                        //data = File.ReadAllBytes(filePath);
                        fstream.Seek(partNumber * Config.FiePartsize, SeekOrigin.Begin);
                        fstream.Read(data, 0, arraySize);
                        return data;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
