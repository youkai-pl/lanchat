using System;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public interface IFileReceiver
    {
        /// <summary>
        ///     Incoming file request.
        /// </summary>
        CurrentFileTransfer CurrentFileTransfer { get; set; }

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        event EventHandler<CurrentFileTransfer> FileReceiveFinished;

        /// <summary>
        ///     File transfer errored.
        /// </summary>
        event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File receive request received.
        /// </summary>
        event EventHandler<CurrentFileTransfer> FileTransferRequestReceived;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        void AcceptRequest();

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        void RejectRequest();

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        void CancelReceive(bool deleteFile);
    }
}