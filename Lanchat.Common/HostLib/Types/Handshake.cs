using System;

namespace Lanchat.Common.HostLib.Types
{
    internal class Handshake
    {
        internal Handshake(string nickname, string publicKey, Guid id, int port)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Id = id;
            Port = port;
        }

        internal Guid Id { get; set; }
        internal string Nickname { get; set; }
        internal int Port { get; set; }
        internal string PublicKey { get; set; }
    }
}