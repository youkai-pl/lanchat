using System;
using System.IO;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    public class FileReceiver
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

        public event EventHandler<FileTransferRequest> FileReceived;
        public event EventHandler<Exception> FileExchangeError;
        public event EventHandler<FileTransferRequest> FileExchangeRequestReceived;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request.Accepted = true;
            writeFileStream = new FileStream(Request.FileName, FileMode.Append);
            networkOutput.SendUserData(DataTypes.FileExchangeRequest, new FileTransferStatus
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
            networkOutput.SendUserData(DataTypes.FileExchangeRequest, new FileTransferStatus
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
                DataTypes.FileExchangeRequest,
                new FileTransferStatus
                {
                    RequestStatus = RequestStatus.Canceled
                });

            File.Delete(Request.FilePath);
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
            FileExchangeRequestReceived?.Invoke(this, Request);
        }

        internal void HandleReceivedFilePart(FilePart filePart)
        {
            if (Request == null) return;
            if (!Request.Accepted) return;

            try
            {
                var data = encryption.Decrypt(filePart.Data);
                writeFileStream.Write(data, 0, data.Length);
                Request.PartsTransferred++;
                if (!filePart.Last) return;
                FileReceived?.Invoke(this, Request);
                writeFileStream.Dispose();
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
            }
        }

        internal void HandleSenderError()
        {
            if (Request == null) return;
            writeFileStream.Dispose();
            File.Delete(Request.FilePath);
            Request = null;
            FileExchangeError?.Invoke(this, new Exception("File transfer cancelled by sender"));
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