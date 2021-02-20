using System;
using System.Collections.Generic;
using System.IO;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    public class FileReceiver : IApiHandler
    {
        private readonly IBytesEncryption encryption;
        private readonly INetworkOutput networkOutput;

        private FileStream writeFileStream;

        internal FileReceiver(INetworkOutput networkOutput, IBytesEncryption encryption)
        {
            this.networkOutput = networkOutput;
            this.encryption = encryption;
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request { get; set; }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.FilePart
        };

        public void Handle(DataTypes type, object data)
        {
            var binary = (FilePart)data;
            HandleReceivedFilePart(binary);
        }

        public event EventHandler<FileTransferRequest> FileTransferFinished;
        public event EventHandler<Exception> FileTransferError;
        public event EventHandler<FileTransferRequest> FileTransferRequestReceived;
        public event EventHandler FileTransferStarted; 

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request.Accepted = true;
            writeFileStream = new FileStream(Request.FileName, FileMode.Append);
            networkOutput.SendUserData(DataTypes.FileTransferStatus, new FileTransferStatus
            {
                RequestStatus = RequestStatus.Accepted
            });
            FileTransferStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request = null;
            networkOutput.SendUserData(DataTypes.FileTransferStatus, new FileTransferStatus
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
                DataTypes.FileTransferStatus,
                new FileTransferStatus
                {
                    RequestStatus = RequestStatus.Canceled
                });

            File.Delete(Request.FilePath);
            FileTransferError?.Invoke(this, new Exception("File transfer cancelled by user"));
            Request = null;
            writeFileStream.Dispose();
        }

        internal void HandleReceiveRequest(FileTransferStatus request)
        {
            Request = new FileTransferRequest
            {
                FilePath = MakeUnique(request.FileName),
                Parts = request.Parts
            };
            FileTransferRequestReceived?.Invoke(this, Request);
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
            }
            catch (Exception e)
            {
                CancelReceive();
                FileTransferError?.Invoke(this, e);
            }
        }

        internal void HandleSenderError()
        {
            if (Request == null) return;
            writeFileStream.Dispose();
            File.Delete(Request.FilePath);
            Request = null;
            FileTransferError?.Invoke(this, new Exception("File transfer cancelled by sender"));
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
    }
}