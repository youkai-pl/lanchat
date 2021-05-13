using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File sending.
    /// </summary>
    public class FileSender
    {
        private readonly FileTransferOutput fileTransferOutput;
        private readonly IStorage storage;
        private const int ChunkSize = 1024 * 1024;
        private bool disposing;

        public FileSender(FileTransferOutput fileTransferOutput, IStorage storage)
        {
            this.fileTransferOutput = fileTransferOutput;
            this.storage = storage;
        }

        /// <summary>
        ///     Outgoing file request.
        /// </summary>
        public CurrentFileTransfer CurrentFileTransfer { get; private set; }

        /// <summary>
        ///     File send returned error.
        /// </summary>
        public event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File send request accepted. File transfer in progress.
        /// </summary>
        public event EventHandler<CurrentFileTransfer> AcceptedByReceiver;

        /// <summary>
        ///     File send request accepted.
        /// </summary>
        public event EventHandler<CurrentFileTransfer> FileTransferRequestRejected;

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<CurrentFileTransfer> FileSendFinished;

        /// <summary>
        ///     Send file exchange request.
        /// </summary>
        /// <param name="path">File path</param>
        /// <exception cref="InvalidOperationException">Only one file can be send at same time</exception>
        public void CreateSendRequest(string path)
        {
            if (CurrentFileTransfer is {Disposed: false})
            {
                throw new InvalidOperationException("File transfer already in progress");
            }

            CurrentFileTransfer = new CurrentFileTransfer
            {
                FilePath = path,
                Parts = (storage.GetFileSize(path) + ChunkSize - 1) / ChunkSize
            };

            fileTransferOutput.SendRequest(CurrentFileTransfer);
        }

        internal void SendFile()
        {
            CurrentFileTransfer.Accepted = true;
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            AcceptedByReceiver?.Invoke(this, CurrentFileTransfer);

            try
            {
                var file = new FileReader(CurrentFileTransfer.FilePath);
                Task.Run(() =>
                {
                    while (file.ReadChunk(ChunkSize, out var chunk))
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

        internal void HandleReject()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }

            FileTransferRequestRejected?.Invoke(this, CurrentFileTransfer);
            CurrentFileTransfer.Dispose();
        }

        internal void HandleError()
        {
            if (CurrentFileTransfer == null ||
                CurrentFileTransfer.Disposed ||
                CurrentFileTransfer.Accepted == false)
            {
                return;
            }

            CurrentFileTransfer.Dispose();
        }

        internal void Dispose()
        {
            CurrentFileTransfer?.Dispose();
            disposing = true;
        }

        private void OnFileTransferError(FileTransferException e)
        {
            FileTransferError?.Invoke(this, e);
        }
    }
}