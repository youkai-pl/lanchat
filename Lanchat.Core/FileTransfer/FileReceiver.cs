using System;
using System.ComponentModel;
using System.IO;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver : INotifyPropertyChanged
    {
        private readonly IConfig config;
        internal readonly IBytesEncryption Encryption;
        internal readonly FileReceiverHandler FileReceiverHandler;
        private readonly INetworkOutput networkOutput;
        private FileTransferRequest fileTransferRequest;
        internal FileStream WriteFileStream;

        internal FileReceiver(INetworkOutput networkOutput, IBytesEncryption encryption, IConfig config)
        {
            this.networkOutput = networkOutput;
            this.config = config;
            Encryption = encryption;
            FileReceiverHandler = new FileReceiverHandler(this);
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request
        {
            get => fileTransferRequest;
            internal set
            {
                if (fileTransferRequest == value) return;
                fileTransferRequest = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Raised on <see cref="Request" /> property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferFinished;

        /// <summary>
        ///     File transfer errored.
        /// </summary>
        public event EventHandler<Exception> FileTransferError;

        /// <summary>
        ///     File receive request received.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestReceived;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request.Accepted = true;
            WriteFileStream = new FileStream(Request.FilePath, FileMode.Append);
            networkOutput.SendUserData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Accepted
            });
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request = null;
            networkOutput.SendUserData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Rejected
            });
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public void CancelReceive()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            networkOutput.SendUserData(
                new FileTransferControl
                {
                    RequestStatus = RequestStatus.Canceled
                });

            File.Delete(Request.FilePath);
            FileTransferError?.Invoke(this, new Exception("File transfer cancelled by user"));
            Request = null;
            WriteFileStream.Dispose();
        }

        internal void HandleReceiveRequest(FileTransferControl request)
        {
            Request = new FileTransferRequest
            {
                FilePath = MakeUnique(Path.Combine(config.ReceivedFilesDirectory, request.FileName)),
                Parts = request.Parts
            };
            FileTransferRequestReceived?.Invoke(this, Request);
        }

        internal void HandleSenderError()
        {
            if (Request == null) return;
            WriteFileStream.Dispose();
            File.Delete(Request.FilePath);
            Request = null;
            OnFileTransferError(new Exception("File transfer cancelled by sender"));
        }

        private static string MakeUnique(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileExt = Path.GetExtension(file);

            for (var i = 1;; ++i)
            {
                if (!File.Exists(file))
                    return file;
                file = $"{fileName}({i}){fileExt}";
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnFileTransferFinished(FileTransferRequest e)
        {
            FileTransferFinished?.Invoke(this, e);
        }

        internal void OnFileTransferError(Exception e)
        {
            FileTransferError?.Invoke(this, e);
        }
    }
}