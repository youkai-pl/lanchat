using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib;
using Lanchat.Common.HostLib.Types;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public class Node
    {
        public Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
            SelfAes = new AesInstance();
        }

        public void CreateConnection()
        {
            Client = new Client(this);
            Client.Connect(Ip, Port);
        }

        public void AcceptHandshake(Handshake handshake)
        {
            Nickname = handshake.Nickname;
            PublicKey = handshake.PublicKey;
            Client.SendKey(new Key(
                Rsa.Encode(SelfAes.Key, PublicKey),
                Rsa.Encode(SelfAes.IV, PublicKey)));
        }

        public void CreateRemoteAes(string key, string iv)
        {
            RemoteAes = new AesInstance(key, iv);
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public bool Mute { get; set; }
        public AesInstance SelfAes { get; set; }
        public AesInstance RemoteAes { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Client { get; set; }
    }
}