using System.Net;

namespace Lanchat.Core.Config
{
    public class SavedNode
    {
        public IPAddress IpAddress { get; set; }
        public byte[] PublicKey { get; set; }
    }
}