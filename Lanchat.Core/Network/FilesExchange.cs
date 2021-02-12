using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class FilesExchange
    {
        public FileExchangeRequest CurrentSendRequest { get; set; }
        public FileExchangeRequest CurrentReceiveRequest { get; set; }

        public event EventHandler<FileExchangeRequest> FileReceived;
        public event EventHandler<FileExchangeRequest> FileExchangeRequestReceived;
        public event EventHandler<Exception> FileExchangeError;

        private readonly Node node;

        public FilesExchange(Node node)
        {
            this.node = node;
        }

        internal FileExchangeRequest CreateSendRequest(string path)
        {
            try
            {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(path);
                CurrentSendRequest = new FileExchangeRequest
                {
                    FilePath = path,
                    FileName = Path.GetFileName(path),
                    RequestStatus = RequestStatus.Sending
                };

                return CurrentSendRequest;
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
                return null;
            }
        }

        internal IEnumerable<FilePart> PrepareFileToSend()
        {
            try
            {
                var file = Convert.ToBase64String(File.ReadAllBytes(CurrentSendRequest.FilePath));
                var list = new List<FilePart>();
                const int chunkSize = 1024 * 1024;
                for (var i = 0; i < file.Length; i += chunkSize)
                {
                    var dataPart = file.Substring(i, Math.Min(chunkSize, file.Length - i));
                    list.Add(new FilePart
                    {
                        Data = dataPart
                    });
                }

                list.Last().Last = true;
                return list;
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
                return null;
            }
        }

        internal void HandleReceivedFile(FilePart file)
        {
            if (CurrentReceiveRequest.RequestStatus != RequestStatus.Accepted)
            {
                return;
            }

            try
            {
                var fileName = Path.GetFileName("tmp" + CurrentReceiveRequest.FileName);
                var data = Convert.FromBase64String(file.Data);
                using var tempFile = new FileStream(fileName, FileMode.Append);
                tempFile.Write(data, 0, data.Length);

                if (!file.Last) return;
                FileReceived?.Invoke(this, node.FilesExchange.CurrentReceiveRequest);
            }
            catch (Exception e)
            {
                FileExchangeError?.Invoke(this, e);
            }
        }

        internal void HandleFileExchangeRequest(FileExchangeRequest request)
        {
            switch (request.RequestStatus)
            {
                case RequestStatus.Accepted:
                    node.NetworkOutput.SendFile();
                    break;

                case RequestStatus.Rejected:
                    CurrentSendRequest = null;
                    break;

                case RequestStatus.Sending:
                    CurrentReceiveRequest = request;
                    FileExchangeRequestReceived?.Invoke(this, request);
                    break;

                default:
                    Trace.Write($"Node {node.Id} received file exchange request of unknown type.");
                    break;
            }
        }
    }
}