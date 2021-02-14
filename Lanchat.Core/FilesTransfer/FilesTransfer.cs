using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.FilesTransfer
{
    public class FilesExchange
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly Node node;

        private FileStream writeFileStream;

        public FilesExchange(Node node)
        {
            this.node = node;
        }

        public FileTransferRequest CurrentSendRequest { get; set; }
        public FileTransferRequest CurrentReceiveRequest { get; set; }

        public event EventHandler<FileTransferRequest> FileReceived;
        public event EventHandler<FileTransferRequest> FileExchangeRequestReceived;
        public event EventHandler FileExchangeRequestAccepted;
        public event EventHandler FileExchangeRequestRejected;
        public event EventHandler<Exception> FileExchangeError;

        public void AcceptRequest()
        {
            CurrentReceiveRequest.Accepted = true;
            writeFileStream = new FileStream(CurrentReceiveRequest.FileName, FileMode.Append);
            node.NetworkOutput.SendFileExchangeAccept();
        }

        public void RejectRequest()
        {
            node.NetworkOutput.SendFileExchangeReject();
        }

        internal FileTransferStatus CreateSendRequest(string path)
        {
            try
            {
                var fileInfo = new FileInfo(path);

                CurrentSendRequest = new FileTransferRequest
                {
                    FilePath = path,
                    Parts = fileInfo.Length / ChunkSize
                };

                return new FileTransferStatus
                {
                    FileName = CurrentSendRequest.FileName,
                    RequestStatus = RequestStatus.Sending,
                    Parts = CurrentSendRequest.Parts
                };
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
                return null;
            }
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