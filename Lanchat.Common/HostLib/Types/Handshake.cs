using System;

namespace Lanchat.Common.HostLib.Types
{
    public class Handshake
    {
        public Handshake(string nickname, string publicKey, Guid id, int port)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Id = id;
            Port = port;
        }

        public string Nickname { get; set; }
        public string PublicKey { get; set; }
        public Guid Id { get; set; }
        public int Port { get; set; }
    }
}