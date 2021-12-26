using System;
using System.IO;
using Lanchat.Core.FileSystem;

namespace Lanchat.Tests.Mock.FileTransfer
{
    public class StorageMock : IStorage
    {
        public long GetFileSize(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public string GetNewFilePath(string file)
        {
            throw new NotImplementedException();
        }

        public void CatchFileSystemExceptions(Exception e, Action errorHandler)
        {
            throw new NotImplementedException();
        }

        public FileStream OpenWriteStream(string path)
        {
            throw new NotImplementedException();
        }
    }
}