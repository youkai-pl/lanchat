using System;
using System.IO;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver : IFileTransfer
    {
        private readonly IConfig config;
        internal readonly IBytesEncryption Encryption;
        private readonly INetworkOutput networkOutput;
        internal FileStream WriteFileStream;

        internal FileReceiver(INetworkOutput networkOutput, IBytesEncryption encryption, IConfig config)
        {
            this.networkOutput = networkOutput;
            this.config = config;
            Encryption = encryption;
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request { get; internal set; }

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileReceiveFinished;

        /// <summary>
        ///     File transfer errored.
        /// </summary>
        public event EventHandler<Exception> FileTransferError;

        /// <summary>
        ///     File receive request received.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestReceived;
        
        /// <summary>
        ///     File send request accepted. File transfer in progress.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestAccepted;

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
            FileTransferRequestAccepted?.Invoke(this, Request);
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

        internal void OnFileTransferFinished(FileTransferRequest e)
        {
            FileReceiveFinished?.Invoke(this, e);
        }

        internal void OnFileTransferError(Exception e)
        {
            FileTransferError?.Invoke(this, e);
        }
    }
}