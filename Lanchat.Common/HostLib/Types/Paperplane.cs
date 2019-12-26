using System;

namespace Lanchat.Common.HostLib.Types
{
    internal class Paperplane
    {
        internal Paperplane(int port, Guid id)
        {
            Port = port;
            Id = id;
        }

        internal Guid Id { get; set; }
        internal int Port { get; set; }
    }
}