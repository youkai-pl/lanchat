using System;

namespace Lanchat.Core.FileTransfer
{
    public class FileTransferException : Exception
    {
        public FileTransferRequest Request { get; }
        
        public FileTransferException(FileTransferRequest request)
        {
            Request = request;
        }
    }
}