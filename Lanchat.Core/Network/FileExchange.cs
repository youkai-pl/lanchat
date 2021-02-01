using System;
using System.Diagnostics;
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
                var fileName = Path.GetFileName(CurrentSendRequest.FilePath);
                CurrentSendRequest = null;
                return new Binary
                {
                    Data = Convert.ToBase64String(file),
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

                File.WriteAllBytes(fileName, Convert.FromBase64String(file.Data));
            }
            catch (Exception e)
            {
                // TODO: Create exception event
            }
        }
    }
}