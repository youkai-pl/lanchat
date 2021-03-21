using System;
using System.ComponentModel;
using System.IO;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    public class FileReceiver : ApiHandler<FilePart>, INotifyPropertyChanged
    {
        private readonly IBytesEncryption encryption;
        private readonly IConfig config;
        private readonly INetworkOutput networkOutput;
        private FileTransferRequest fileTransferRequest;

        private FileStream writeFileStream;

        internal FileReceiver(INetworkOutput networkOutput, IBytesEncryption encryption, IConfig config)
        {
            this.networkOutput = networkOutput;
            this.encryption = encryption;
            this.config = config;
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request
        {
            get => fileTransferRequest;
            private set
            {
                if (fileTransferRequest == value) return;
                fileTransferRequest = value;
                OnPropertyChanged();
            }
        }

        protected override void Handle(FilePart binary)
        {
            HandleReceivedFilePart(binary);
        }

        public event EventHandler<FileTransferRequest> FileTransferFinished;
        public event EventHandler<Exception> FileTransferError;
        public event EventHandler<FileTransferRequest> FileTransferRequestReceived;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request.Accepted = true;
            writeFileStream = new FileStream(Request.FilePath, FileMode.Append);
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
            writeFileStream.Dispose();
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
            writeFileStream.Dispose();
            File.Delete(Request.FilePath);
            Request = null;
            FileTransferError?.Invoke(this, new Exception("File transfer cancelled by sender"));
        }

        private void HandleReceivedFilePart(FilePart filePart)
        {
            if (Request == null) return;
            if (!Request.Accepted) return;

            try
            {
                var data = encryption.Decrypt(filePart.Data);
                writeFileStream.Write(data, 0, data.Length);
                Request.PartsTransferred++;
                if (!filePart.Last) return;
                FileTransferFinished?.Invoke(this, Request);
                writeFileStream.Dispose();
                Request = null;
            }
            catch (Exception e)
            {
                CancelReceive();
                FileTransferError?.Invoke(this, e);
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}