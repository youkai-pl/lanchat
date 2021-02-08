using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class FileExchange
    {
        public FileExchangeRequest CurrentSendRequest { get; set; }
        public FileExchangeRequest CurrentReceiveRequest { get; set; }

        public event EventHandler<FileExchangeRequest> FileReceived;

        private readonly Node node;

        public FileExchange(Node node)
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
                    Checksum = string.Concat(md5.ComputeHash(stream).Select(x => x.ToString("X2"))),
                    FilePath = path,
                    RequestStatus = RequestStatus.Sending
                };

                return CurrentSendRequest;
            }
            catch (Exception e)
            {
                // TODO: Create exception event
                return null;
            }
        }

        internal Binary PrepareFileToSend()
        {
            try
            {
                var file = File.ReadAllBytes(CurrentSendRequest.FilePath);
                var encrypted = node.Encryption.Encrypt(Convert.ToBase64String(file));
                var fileName = Path.GetFileName(CurrentSendRequest.FilePath);
                CurrentSendRequest = null;
                return new Binary
                {
                    Data = encrypted,
                    Filename = fileName
                };
            }
            catch (Exception e)
            {
                // TODO: Create exception event
                return null;
            }
        }

        internal void HandleReceivedFile(Binary file)
        {
            if (CurrentReceiveRequest.RequestStatus != RequestStatus.Accepted)
            {
                return;
            }

            try
            {
                var fileName = Path.GetFileName(file.Filename);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "unknown_file_from_lanchat";
                }

                if (File.Exists(fileName))
                {
                    fileName =
                        $"{Path.GetFileNameWithoutExtension(fileName)}({DateTime.Now:HH_mm_ss}){Path.GetExtension(fileName)}";
                }

                var decrypted = Convert.FromBase64String(node.Encryption.Decrypt(file.Data));
                File.WriteAllBytes(fileName, decrypted);
                FileReceived?.Invoke(this, node.FileExchange.CurrentReceiveRequest);
            }
            catch (Exception e)
            {
                // TODO: Create exception event
            }
        }
    }
}