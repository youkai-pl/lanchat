using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core.FilesTransfer
{
    public class FilesExchange
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly Node node;

        private FileStream writeFileStream;

        internal FilesExchange(Node node)
        {
            this.node = node;
        }

        /// <summary>
        ///    Outgoing file request. 
        /// </summary>
        public FileTransferRequest CurrentSendRequest { get; set; }

        /// <summary>
        ///     Incoming file request.  
        /// </summary>
        public FileTransferRequest CurrentReceiveRequest { get; set; }

        public event EventHandler<FileTransferRequest> FileReceived;
        public event EventHandler<FileTransferRequest> FileExchangeRequestReceived;
        public event EventHandler FileExchangeRequestAccepted;
        public event EventHandler FileExchangeRequestRejected;
        public event EventHandler<Exception> FileExchangeError;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (CurrentReceiveRequest == null) throw new InvalidOperationException("No receive request");
            CurrentReceiveRequest.Accepted = true;
            writeFileStream = new FileStream(CurrentReceiveRequest.FileName, FileMode.Append);
            node.NetworkOutput.SendFileExchangeAccept();
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (CurrentReceiveRequest == null) throw new InvalidOperationException("No receive request");
            CurrentReceiveRequest = null;
            node.NetworkOutput.SendFileExchangeReject();
        }

        /// <summary>
        ///     Send file exchange request.
        /// </summary>
        /// <param name="path">File path</param>
        public void CreateSendRequest(string path)
        {
            if (CurrentSendRequest != null) throw new InvalidOperationException("File transfer in progress.");

            var fileInfo = new FileInfo(path);

            CurrentSendRequest = new FileTransferRequest
            {
                FilePath = path,
                Parts = fileInfo.Length / ChunkSize
            };

            node.NetworkOutput.SendData(
                DataTypes.FileExchangeRequest,
                new FileTransferStatus
                {
                    FileName = CurrentSendRequest.FileName,
                    RequestStatus = RequestStatus.Sending,
                    Parts = CurrentSendRequest.Parts
                });
        }

        internal void HandleReceivedFilePart(FilePart filePart)
        {
            if (!CurrentReceiveRequest.Accepted) return;

            try
            {
                var data = node.Encryption.Decrypt(filePart.Data);
                writeFileStream.Write(data, 0, data.Length);
                CurrentReceiveRequest.PartsTransferred++;
                if (!filePart.Last) return;
                FileReceived?.Invoke(this, CurrentReceiveRequest);
                writeFileStream.Dispose();
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
            }
        }

        internal void HandleFileExchangeRequest(FileTransferStatus request)
        {
            switch (request.RequestStatus)
            {
                case RequestStatus.Accepted:
                    FileExchangeRequestAccepted?.Invoke(this, EventArgs.Empty);
                    SendFile();
                    break;

                case RequestStatus.Rejected:
                    FileExchangeRequestRejected?.Invoke(this, EventArgs.Empty);
                    CurrentSendRequest = null;
                    break;

                case RequestStatus.Sending:
                    CurrentReceiveRequest = new FileTransferRequest
                    {
                        FilePath = MakeUnique(request.FileName),
                        Parts = request.Parts
                    };
                    FileExchangeRequestReceived?.Invoke(this, CurrentReceiveRequest);
                    break;

                case RequestStatus.Errored:
                    if (CurrentReceiveRequest != null)
                    {
                        CurrentReceiveRequest = null;
                        FileExchangeError?.Invoke(this, new Exception("File transfer cancelled by sending site"));
                    }
                    break;
                
                default:
                    Trace.Write($"Node {node.Id} received file exchange request of unknown type.");
                    break;
            }
        }

        private void SendFile()
        {
            try
            {
                var buffer = new byte[ChunkSize];
                int bytesRead;
                using var file = File.OpenRead(CurrentSendRequest.FilePath);
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var part = new FilePart
                    {
                        Data = node.Encryption.Encrypt(buffer.Take(bytesRead).ToArray())
                    };

                    if (bytesRead < ChunkSize) part.Last = true;

                    node.NetworkOutput.SendData(DataTypes.FilePart, part);
                    CurrentSendRequest.PartsTransferred++;
                }
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
                node.NetworkOutput.SendData(
                    DataTypes.FileExchangeRequest,
                    new FileTransferStatus
                    {
                        RequestStatus = RequestStatus.Errored
                    });
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
    }
}