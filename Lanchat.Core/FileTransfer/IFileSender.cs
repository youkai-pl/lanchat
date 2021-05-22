using System;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File sending.
    /// </summary>
    public interface IFileSender
    {
        /// <summary>
        ///     Outgoing file request.
        /// </summary>
        CurrentFileTransfer CurrentFileTransfer { get; }

        /// <summary>
        ///     File send returned error.
        /// </summary>
        event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File send request accepted. File transfer in progress.
        /// </summary>
        event EventHandler<CurrentFileTransfer> AcceptedByReceiver;

        /// <summary>
        ///     File send request accepted.
        /// </summary>
        event EventHandler<CurrentFileTransfer> FileTransferRequestRejected;

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        event EventHandler<CurrentFileTransfer> FileSendFinished;

        /// <summary>
        ///     Send file exchange request.
        /// </summary>
        /// <param name="path">File path</param>
        /// <exception cref="InvalidOperationException">Only one file can be send at same time</exception>
        void CreateSendRequest(string path);
    }
}