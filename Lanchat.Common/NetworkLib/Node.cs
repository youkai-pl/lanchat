using Lanchat.Common.HostLib;
using System;
using System.Net;
using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib.Types;

namespace Lanchat.Common.NetworkLib
{
    public class Node
    {
        public Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
            Aes = new AesInstance();
        }

        public void CreateConnection()
        {
            Client = new Client();
            Client.Connect(Ip, Port);
        }

        public void AcceptHandshake(Handshake handshake)
        {
            Nickname = handshake.Nickname;
            PublicKey = handshake.PublicKey;
            Client.SendKey(new Key(
                Rsa.Encode(Aes.Key, PublicKey),
                Rsa.Encode(Aes.IV, PublicKey)));
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public AesInstance Aes { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Client { get; set; }
    }
}