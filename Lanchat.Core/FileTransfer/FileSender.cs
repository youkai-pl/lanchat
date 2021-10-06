using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileSender : IFileSender, IInternalFileSender, IDisposable
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly FileTransferOutput fileTransferOutput;
        private readonly IStorage storage;
        private bool disposing;

        public FileSender(FileTransferOutput fileTransferOutput, IStorage storage)
        {
            this.fileTransferOutput = fileTransferOutput;
            this.storage = storage;
        }

        public CurrentFileTransfer CurrentFileTransfer { get; private set; }

        public event EventHandler<FileTransferException> FileTransferError;
        public event EventHandler<CurrentFileTransfer> FileTransferQueued;
        public event EventHandler<CurrentFileTransfer> AcceptedByReceiver;
        public event EventHandler<CurrentFileTransfer> FileTransferRequestRejected;
        public event EventHandler<CurrentFileTransfer> FileSendFinished;
        
        public void CreateSendRequest(string path)
        {
            if (CurrentFileTransfer is { Disposed: false })
            {
                throw new InvalidOperationException("File transfer already in progress");
            }

            CurrentFileTransfer = new CurrentFileTransfer
            {
                FilePath = path,
                Parts = (storage.GetFileSize(path) + ChunkSize - 1) / ChunkSize
            };

            fileTransferOutput.SendRequest(CurrentFileTransfer);
            FileTransferQueued?.Invoke(this, CurrentFileTransfer);
        }

        public void SendFile()
        {
            CurrentFileTransfer.Accepted = true;
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            AcceptedByReceiver?.Invoke(this, CurrentFileTransfer);

            try
            {
                var file = new FileReader(ChunkSize, CurrentFileTransfer.FilePath);
                Task.Run(() =>
                {
                    while (file.ReadChunk(out var chunk))
                    {
                        if (disposing || CurrentFileTransfer.Disposed)
                        {
                            OnFileTransferError(new FileTransferException(CurrentFileTransfer, "Cancelled"));
                            return;
                        }

                        var part = new FilePart
                        {
                            Data = Convert.ToBase64String(chunk)
                        };

                        fileTransferOutput.SendPart(part);
                        CurrentFileTransfer.PartsTransferred++;
                    }

                    FileSendFinished?.Invoke(this, CurrentFileTransfer);
                    fileTransferOutput.SendSignal(FileTransferStatus.Finished);
                    CurrentFileTransfer.Dispose();
                });
            }
            catch (Exception e)
            {
                storage.CatchFileSystemException(e, () =>
                {
                    OnFileTransferError(new FileTransferException(CurrentFileTransfer, e.Message));
                    fileTransferOutput.SendSignal(FileTransferStatus.SenderError);
                    CurrentFileTransfer = null;
                    Trace.WriteLine("Cannot access file system");
                });
            }
        }

        public void HandleReject()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            FileTransferRequestRejected?.Invoke(this, CurrentFileTransfer);
            CurrentFileTransfer.Dispose();
        }

        public void HandleError()
        {
            if (CurrentFileTransfer == null ||
                CurrentFileTransfer.Disposed ||
                CurrentFileTransfer.Accepted == false)
            {
                return;
            }

            CurrentFileTransfer.Dispose();
        }

        public void CancelSend()
        {
            CurrentFileTransfer?.Dispose();
            fileTransferOutput.SendSignal(FileTransferStatus.SenderError);
        }
        
        public void Dispose()
        {
            if (CurrentFileTransfer != null && CurrentFileTransfer.Progress != 100)
            {
                CurrentFileTransfer.Dispose();
                FileTransferError?.Invoke(this, new FileTransferException(
                    CurrentFileTransfer, 
                    "User disconnected before file transfer ended"));
            }
            disposing = true;
        }

        private void OnFileTransferError(FileTransferException e)
        {
            FileTransferError?.Invoke(this, e);
        }
    }
}