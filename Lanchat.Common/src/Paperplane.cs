using System;

namespace Lanchat.Common.ClientLib
{
    // Paperplane class
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