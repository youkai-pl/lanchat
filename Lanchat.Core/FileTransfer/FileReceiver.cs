using System;
using System.IO;
using Lanchat.Core.Api;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver
    {
        private readonly FileTransferSignalling fileTransferSignalling;
        private readonly IFileSystem fileSystem;
        internal FileStream WriteFileStream;

        internal FileReceiver(IOutput output, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            fileTransferSignalling = new FileTransferSignalling(output);
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
            if (CurrentFileTransfer == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer.Accepted = true;
            WriteFileStream = fileSystem.OpenWriteStream(CurrentFileTransfer.FilePath);
            fileTransferSignalling.SignalAccept();
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (CurrentFileTransfer == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer = null;
            fileTransferSignalling.SignalReject();
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public void CancelReceive(bool deleteFile)
        {
            if (CurrentFileTransfer == null)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileTransferSignalling.SignalCancel();
            if (deleteFile)
            {
                fileSystem.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(CurrentFileTransfer));
            ResetRequest();
        }

        internal void FinishReceive()
        {
            if (CurrentFileTransfer == null)
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, CurrentFileTransfer);
            ResetRequest();
        }

        internal void OnFileTransferRequestReceived()
        {
            FileTransferRequestReceived?.Invoke(this, CurrentFileTransfer);
        }

        internal void HandleSenderError()
        {
            if (CurrentFileTransfer == null)
            {
                return;
            }

            fileSystem.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            OnFileTransferError();
            ResetRequest();
        }

        internal void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(CurrentFileTransfer));
        }

        private void ResetRequest()
        {
            CurrentFileTransfer = null;
            WriteFileStream.Dispose();
        }
    }
}