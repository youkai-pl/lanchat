using Lanchat.Common.HostLib;
using System;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Common.CryptographyLib;

namespace Lanchat.Common.NetworkLib
{
    public class Node
    {
        public Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
            Aes = Cryptography.CreateAesInstance();
        }

        public void CreateConnection()
        {
            Connection = new Client();
            Connection.Connect(Ip, Port);
        }

        public void AcceptHandshake(Handshake handshake)
        {
            Nickname = handshake.Nickname;
            PublicKey = handshake.PublicKey;
            Connection.SendKey(PublicKey, "test");
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public AesManaged Aes { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Connection { get; set; }
    }
}