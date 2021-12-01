using System;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReceiver : IDisposable, IFileReceiver, IInternalFileReceiver
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
            if (CurrentFileTransfer is { Accepted: true, Disposed: false })
            {
                CancelReceive(true);
            }
            else
            {
                CurrentFileTransfer?.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public CurrentFileTransfer CurrentFileTransfer { get; set; }

        public event EventHandler<CurrentFileTransfer> FileReceiveFinished;
        public event EventHandler<FileTransferException> FileTransferError;
        public event EventHandler<CurrentFileTransfer> FileTransferRequestReceived;

        public void AcceptRequest()
        {
            if (CurrentFileTransfer?.Disposed != false)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            CurrentFileTransfer.Accepted = true;
            FileWriter = new FileWriter(CurrentFileTransfer.FilePath);
            fileTransferOutput.SendSignal(FileTransferStatus.Accepted);
        }

        public void RejectRequest()
        {
            if (CurrentFileTransfer is { Disposed: true })
            {
                throw new InvalidOperationException("No pending requests ");
            }

            if (CurrentFileTransfer.Accepted)
            {
                CancelReceive(true);
            }
            else
            {
                CurrentFileTransfer = null;
                fileTransferOutput.SendSignal(FileTransferStatus.Rejected);
            }
        }

        public void CancelReceive(bool deleteFile)
        {
            if (CurrentFileTransfer?.Disposed != false || !CurrentFileTransfer.Accepted)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileTransferOutput.SendSignal(FileTransferStatus.ReceiverError);
            if (deleteFile)
            {
                storage.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(CurrentFileTransfer, "Cancelled"));

            CurrentFileTransfer.Dispose();
            FileWriter.Dispose();
        }

        public FileWriter FileWriter { get; private set; }

        public void FinishReceive()
        {
            if (CurrentFileTransfer is { Disposed: true })
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, CurrentFileTransfer);
            CurrentFileTransfer.Dispose();
            FileWriter.Dispose();
        }

        public void HandleError()
        {
            if (CurrentFileTransfer?.Disposed != false)
            {
                return;
            }

            storage.DeleteIncompleteFile(CurrentFileTransfer.FilePath);
            OnFileTransferError();
            CurrentFileTransfer?.Dispose();
            FileWriter?.Dispose();
        }

        public void OnFileTransferRequestReceived()
        {
            FileTransferRequestReceived?.Invoke(this, CurrentFileTransfer);
        }

        public void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(CurrentFileTransfer, "Sender error"));
        }
    }
}