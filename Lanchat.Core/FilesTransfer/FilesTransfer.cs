using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.FilesTransfer
{
    public class FilesExchange
    {
        private readonly Node node;

        public FilesExchange(Node node)
        {
            this.node = node;
        }

        public FileTransferRequest CurrentSendRequest { get; set; }
        public FileTransferRequest CurrentReceiveRequest { get; set; }

        public event EventHandler<FileTransferRequest> FileReceived;
        public event EventHandler<FileTransferRequest> FileExchangeRequestReceived;
        public event EventHandler<Exception> FileExchangeError;

        public void AcceptRequest()
        {
            CurrentReceiveRequest.Accepted = true;
            node.NetworkOutput.SendFileExchangeAccept();
        }

        public void DenyRequest()
        {
            CurrentReceiveRequest.Accepted = false;
        }

        internal FileTransferStatus CreateSendRequest(string path)
        {
            try
            {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(path);
                CurrentSendRequest = new FileTransferRequest
                {
                    FilePath = path
                };

                return new FileTransferStatus
                {
                    FileName = CurrentSendRequest.FileName,
                    RequestStatus = RequestStatus.Sending
                };
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
                return null;
            }
        }

        internal void HandleReceivedFile(FilePart filePart)
        {
            if (!CurrentReceiveRequest.Accepted) return;

            try
            {
                var fileName = Path.GetFileName("tmp" + CurrentReceiveRequest.FileName);
                var data = filePart.Data;
                using var tempFile = new FileStream(fileName, FileMode.Append);
                tempFile.Write(data, 0, data.Length);

                if (!filePart.Last) return;
                FileReceived?.Invoke(this, node.FilesExchange.CurrentReceiveRequest);
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
                    SendFile();
                    break;

                case RequestStatus.Rejected:
                    CurrentSendRequest = null;
                    break;

                case RequestStatus.Sending:
                    CurrentReceiveRequest = new FileTransferRequest
                    {
                        FilePath = request.FileName
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
                const int chunkSize = 1024 * 1024;
                var buffer = new byte[chunkSize];
                int bytesRead;
                using var file = File.OpenRead(CurrentSendRequest.FilePath);
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    var part = new FilePart
                    {
                        Data = buffer.Take(bytesRead).ToArray()
                    };

                    if (bytesRead < chunkSize)
                    {
                        part.Last = true;
                    }
                    
                    node.NetworkOutput.SendData(DataTypes.FilePart, part);
                }
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
            }
        }
    }
}