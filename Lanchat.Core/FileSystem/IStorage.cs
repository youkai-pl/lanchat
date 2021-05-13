using System;

namespace Lanchat.Core.FileSystem
{
    public interface IStorage
    {
        string GetFilePath(string path);
        long GetFileSize(string path);
        void DeleteIncompleteFile(string path);
        void CatchFileSystemException(Exception e, Action errorHandler);
    }
}