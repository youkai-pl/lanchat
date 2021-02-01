using System;
using System.IO;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class FileExchange
    {
        public FileExchangeRequest CurrentSendRequest { get; set; }
        public FileExchangeRequest CurrentReceiveRequest { get; set; }
        
        public FileExchangeRequest CreateSendRequest(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);
            CurrentSendRequest = new FileExchangeRequest
            {
                Checksum = md5.ComputeHash(stream).ToString(),
                RequestType = FileExchangeRequestType.Sending
            };
            
            return CurrentSendRequest;
        }
        
        public Binary LoadSendRequest()
        {
            var file = File.ReadAllBytes(CurrentSendRequest.FilePath);
            return new Binary
            {
                Data = Convert.ToBase64String(file),
                Filename = Path.GetFileName(CurrentSendRequest.FilePath)
            };
        }
    }
}