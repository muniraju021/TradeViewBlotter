using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Utilities
{
    public interface IFileHelper
    {
        bool CopyFile(string sourceFilePath, string destinationFilePath, bool isOverrideEnabled);
        Task<string[]> ReadAllLines(string filePath);
        FileStream GetFileStream(string filePath);
        string ReadAllText(string filePath);
        bool FileExists(string path);
        void CreateDirectoryIfNotExists(string path);
    }
}
