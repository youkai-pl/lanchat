using System;
using System.IO;
using System.Linq;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    public class FileSender
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly IBytesEncryption encryption;
        private readonly INetworkOutput networkOutput;

        internal FileSender(INetworkOutput networkOutput, IBytesEncryption encryption)
        {
            this.networkOutput = networkOutput;
            this.encryption = encryption;
        }

        /// <summary>
        ///     Outgoing file request.
        /// </summary>
        public FileTransferRequest Request { get; set; }

        public event EventHandler<Exception> FileTransferError;
        public event EventHandler FileTransferRequestAccepted;
        public event EventHandler FileTransferRequestRejected;
        public event EventHandler FileTransferFinished;

        /// <summary>
        ///     Send file exchange request.
        /// </summary>
        /// <param name="path">File path</param>
        public void CreateSendRequest(string path)
        {
            if (Request != null) throw new InvalidOperationException("File transfer in progress");

            var fileInfo = new FileInfo(path);

            Request = new FileTransferRequest
            {
                FilePath = path,
                Parts = fileInfo.Length / ChunkSize
            };

            networkOutput.SendUserData(
                DataTypes.FileTransferStatus,
                new FileTransferStatus
                {
                    FileName = Request.FileName,
                    RequestStatus = RequestStatus.Sending,
                    Parts = Request.Parts
                });
        }

        internal void SendFile()
        {
            FileTransferRequestAccepted?.Invoke(this, EventArgs.Empty);

            try
            {
                var buffer = new byte[ChunkSize];
                int bytesRead;
                using var file = File.OpenRead(Request.FilePath);
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0 && Request != null)
                {
                    var part = new FilePart
                    {
                        Data = encryption.Encrypt(buffer.Take(bytesRead).ToArray())
                    };

                    if (bytesRead < ChunkSize) part.Last = true;
                    networkOutput.SendUserData(DataTypes.FilePart, part);
                    Request.PartsTransferred++;
                }
                FileTransferFinished?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                FileTransferError?.Invoke(this, e);
                networkOutput.SendUserData(
                    DataTypes.FileTransferStatus,
                    new FileTransferStatus
                    {
                        RequestStatus = RequestStatus.Errored
                    });
            }
        }

        internal void HandleReject()
        {
            FileTransferRequestRejected?.Invoke(this, EventArgs.Empty);
            Request = null;
        }

        internal void HandleCancel()
        {
            if (Request == null) return;
            Request = null;
            FileTransferError?.Invoke(this, new Exception("File transfer cancelled by receiver"));
        }
    }
}