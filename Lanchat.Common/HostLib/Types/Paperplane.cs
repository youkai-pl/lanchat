using System;

namespace Lanchat.Common.HostLib.Types
{
    public class Paperplane
    {
        public Paperplane(int port, Guid id)
        {
            Port = port;
            Id = id;
        }

        public int Port { get; set; }
        public Guid Id { get; set; }
    }
}