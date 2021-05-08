using System;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver
    {
        private readonly FileTransferOutput fileTransferOutput;
        private readonly IFileSystem fileSystem;

        internal FileReceiver(FileTransferOutput fileTransferOutput, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            this.fileTransferOutput = fileTransferOutput;
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public CurrentFileTransfer CurrentFileTransfer { get; internal set; }

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<CurrentFileTransfer> FileReceiveFinished;

        /// <summary>
        ///     File transfer errored.
        /// </summary>
        public event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File receive request received.
        /// </summary>
        public event EventHandler<CurrentFileTransfer> FileTransferRequestReceived;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer.Accepted = true;
            CurrentFileTransfer.FileStream = fileSystem.OpenWriteStream(CurrentFileTransfer.FilePath);
            fileTransferOutput.SendSignal(FileTransferStatus.Accepted);
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (CurrentFileTransfer is {Disposed: true})
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer = null;
            fileTransferOutput.SendSignal(FileTransferStatus.Rejected);
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public void CancelReceive(bool deleteFile)
        {
            if (CurrentFileTransfer == null ||
                CurrentFileTransfer.Disposed ||
                !CurrentFileTransfer.Accepted)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileTransferOutput.SendSignal(FileTransferStatus.ReceiverError);
            CurrentFileTransfer.Dispose();
            if (deleteFile)
            {
                fileSystem.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(
                CurrentFileTransfer, 
                "Cancelled by user"));
        }

        internal void FinishReceive()
        {
            if (CurrentFileTransfer is {Disposed: true})
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, CurrentFileTransfer);
            CurrentFileTransfer.Dispose();
        }

        internal void HandleError()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            fileSystem.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            OnFileTransferError();
            CurrentFileTransfer.Dispose();
        }

        internal void OnFileTransferRequestReceived()
        {
            FileTransferRequestReceived?.Invoke(this, CurrentFileTransfer);
        }

        internal void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(
                CurrentFileTransfer, 
                "Error at the sender"));
        }
    }
}