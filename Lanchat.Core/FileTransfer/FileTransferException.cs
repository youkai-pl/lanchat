using System;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     Throw when file exchange stopped before finish.
    /// </summary>
    public class FileTransferException : Exception
    {
        internal FileTransferException(CurrentFileTransfer request)
        {
            Request = request;
        }

        /// <summary>
        ///     Request that throws error.
        /// </summary>
        public CurrentFileTransfer Request { get; }
    }
}