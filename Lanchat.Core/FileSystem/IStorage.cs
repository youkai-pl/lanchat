using System;

namespace Lanchat.Core.FileSystem
{
    public interface IStorage
    {
        string GetNewFilePath(string path);
        long GetFileSize(string path);
        void DeleteFile(string path);
        void CatchFileSystemExceptions(Exception e, Action errorHandler);
    }
}