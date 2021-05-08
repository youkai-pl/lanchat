using System;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    internal interface IFileSystem
    {
        void DeleteIncompleteFile(string path);
        string GetFilePath(string file);
        void CatchFileSystemException(Exception e, Action errorHandler);
    }
}