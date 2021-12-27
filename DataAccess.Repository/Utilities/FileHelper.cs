using DataAccess.Repository.LogServices;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Utilities
{
    public class FileHelper : IFileHelper
    {
        private static ILog _log = LogService.GetLogger(typeof(FileHelper));
        public bool CopyFile(string sourceFilePath, string destinationFilePath, bool isOverrideEnabled)
        {
            try
            {
                File.Copy(sourceFilePath, destinationFilePath, isOverrideEnabled);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"FileHelper: CopyFile Failed - {ex}");
            }
            return false;
            
        }

        public Task<string[]> ReadAllLines(string filePath)
        {
            return File.ReadAllLinesAsync(filePath);
        }

        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public FileStream GetFileStream(string filePath)
        {
            return File.OpenRead(filePath);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void CreateDirectoryIfNotExists(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}
