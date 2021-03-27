using System;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    /// Throw when file exchange stopped before finish.
    /// </summary>
    public class FileTransferException : Exception
    {
        /// <summary>
        /// Request that throws error.
        /// </summary>
        public FileTransferRequest Request { get; }

        internal FileTransferException(FileTransferRequest request)
        {
            Request = request;
        }
    }
}