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
        public event EventHandler<Exception> FileExchangeError;

        public void AcceptRequest()
        {
            CurrentReceiveRequest.Accepted = true;
            writeFileStream = new FileStream(CurrentReceiveRequest.FileName, FileMode.Append);
            node.NetworkOutput.SendFileExchangeAccept();
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

        internal void HandleReceivedFilePart(FilePart filePart)
        {
            if (!CurrentReceiveRequest.Accepted) return;

            try
            {
                var data = filePart.Data;
                writeFileStream.Write(data, 0, data.Length);
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
                    SendFile();
                    break;

                case RequestStatus.Rejected:
                    CurrentSendRequest = null;
                    break;

                case RequestStatus.Sending:
                    CurrentReceiveRequest = new FileTransferRequest
                    {
                        FilePath = MakeUnique(request.FileName)
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

                    if (bytesRead < chunkSize) part.Last = true;

                    node.NetworkOutput.SendData(DataTypes.FilePart, part);
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