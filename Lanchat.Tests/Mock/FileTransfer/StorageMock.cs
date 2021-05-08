using System;
using System.IO;
using Lanchat.Core.FileSystem;

namespace Lanchat.Tests.Mock.FileTransfer
{
    public class StorageMock : IStorage
    {
        public FileStream OpenWriteStream(string path)
        {
            throw new NotImplementedException();
        }

        public long GetFileSize(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteIncompleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFilePath(string file)
        {
            throw new NotImplementedException();
        }

        public void CatchFileSystemException(Exception e, Action errorHandler)
        {
            throw new NotImplementedException();
        }
    }
}