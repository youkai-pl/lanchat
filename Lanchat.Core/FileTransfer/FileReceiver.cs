using System;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReceiver : IDisposable, IInternalFileReceiver
    {
        private readonly FileTransferOutput fileTransferOutput;
        private readonly IStorage storage;

        public FileReceiver(FileTransferOutput fileTransferOutput, IStorage storage)
        {
            this.storage = storage;
            this.fileTransferOutput = fileTransferOutput;
        }

        public void Dispose()
        {
            if (CurrentFileTransfer is {Accepted: true, Disposed: false})
            {
                CancelReceive(true);
            }
            else
            {
                CurrentFileTransfer?.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public FileWriter FileWriter { get; private set; }
        public CurrentFileTransfer CurrentFileTransfer { get; set; }

        public event EventHandler<CurrentFileTransfer> FileReceiveFinished;

        public event EventHandler<FileTransferException> FileTransferError;

        public event EventHandler<CurrentFileTransfer> FileTransferRequestReceived;

        public void AcceptRequest()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer.Accepted = true;
            FileWriter = new FileWriter(CurrentFileTransfer.FilePath);
            fileTransferOutput.SendSignal(FileTransferStatus.Accepted);
        }

        public void RejectRequest()
        {
            if (CurrentFileTransfer is {Disposed: true})
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer = null;
            fileTransferOutput.SendSignal(FileTransferStatus.Rejected);
        }

        public void CancelReceive(bool deleteFile)
        {
            if (CurrentFileTransfer == null ||
                CurrentFileTransfer.Disposed ||
                !CurrentFileTransfer.Accepted)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileTransferOutput.SendSignal(FileTransferStatus.ReceiverError);
            if (deleteFile)
            {
                storage.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(
                CurrentFileTransfer,
                "Cancelled by user"));
            CurrentFileTransfer.Dispose();
        }

        public void FinishReceive()
        {
            if (CurrentFileTransfer is {Disposed: true})
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, CurrentFileTransfer);
            CurrentFileTransfer.Dispose();
        }

        public void HandleError()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            storage.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            OnFileTransferError();
            CurrentFileTransfer.Dispose();
        }

        public void OnFileTransferRequestReceived()
        {
            FileTransferRequestReceived?.Invoke(this, CurrentFileTransfer);
        }

        public void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(
                CurrentFileTransfer,
                "Error at the sender"));
        }
    }
}