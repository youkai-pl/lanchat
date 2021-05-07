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
        public CurrentFileTransfer Request { get; internal set; }

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
            if (Request == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            Request.Accepted = true;
            WriteFileStream = fileSystem.OpenWriteStream(Request.FilePath);
            fileTransferSignalling.SignalAccept();
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (Request == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            Request = null;
            fileTransferSignalling.SignalReject();
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public void CancelReceive(bool deleteFile)
        {
            if (Request == null)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileTransferSignalling.SignalCancel();
            if (deleteFile)
            {
                fileSystem.DeleteIncompleteFile(Request.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(Request));
            ResetRequest();
        }

        internal void FinishReceive()
        {
            if (Request == null)
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, Request);
            ResetRequest();
        }

        internal void OnFileTransferRequestReceived()
        {
            FileTransferRequestReceived?.Invoke(this, Request);
        }

        internal void HandleSenderError()
        {
            if (Request == null)
            {
                return;
            }

            fileSystem.DeleteIncompleteFile(Request.FilePath);
            OnFileTransferError();
            ResetRequest();
        }

        internal void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(Request));
        }

        private void ResetRequest()
        {
            Request = null;
            WriteFileStream.Dispose();
        }
    }
}