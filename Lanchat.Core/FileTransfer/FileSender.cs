using System;
using System.Diagnostics;
using System.IO;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File sending.
    /// </summary>
    public class FileSender
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly FileTransferOutput fileTransferOutput;
        private bool disposing;

        internal FileSender(FileTransferOutput fileTransferOutput)
        {
            this.fileTransferOutput = fileTransferOutput;
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

            var fileInfo = new FileInfo(Path.Combine(path));

            CurrentFileTransfer = new CurrentFileTransfer
            {
                FilePath = path,
                Parts = (fileInfo.Length + ChunkSize - 1) / ChunkSize
            };

            fileTransferOutput.SendRequest(CurrentFileTransfer);
        }

        internal void SendFile()
        {
            if (CurrentFileTransfer == null || CurrentFileTransfer.Disposed)
            {
                return;
            }
            
            AcceptedByReceiver?.Invoke(this, CurrentFileTransfer);

            try
            {
                var file = new FileReader(CurrentFileTransfer, ChunkSize);
                do
                {
                    if (disposing)
                    {
                        OnFileTransferError(new FileTransferException(CurrentFileTransfer));
                        return;
                    }

                    var part = new FilePart
                    {
                        Data = Convert.ToBase64String(file.ReadChunk())
                    };

                    fileTransferOutput.SendPart(part);
                    CurrentFileTransfer.PartsTransferred++;
                } while (!file.EndReached);

                FileSendFinished?.Invoke(this, CurrentFileTransfer);
                fileTransferOutput.SendSignal(FileTransferStatus.Finished);
                CurrentFileTransfer.Dispose();
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
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

        internal void HandleCancel()
        {
            if (CurrentFileTransfer == null || 
                CurrentFileTransfer.Disposed ||
                CurrentFileTransfer.Accepted == false)
            {
                return;
            }

            OnFileTransferError(new FileTransferException(CurrentFileTransfer));
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

        private void CatchFileSystemExceptions(Exception e)
        {
            if (e is not (
                DirectoryNotFoundException or
                FileNotFoundException or
                IOException or
                UnauthorizedAccessException))
            {
                throw e;
            }

            OnFileTransferError(new FileTransferException(CurrentFileTransfer));
            fileTransferOutput.SendSignal(FileTransferStatus.Errored);
            CurrentFileTransfer = null;
            Trace.WriteLine("Cannot access file system");
        }
    }
}