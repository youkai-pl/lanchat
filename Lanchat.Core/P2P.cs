using System.Net;

namespace Lanchat.Core
{
    public class P2P
    {
        public Server Server { get; }
        
        public P2P(int port)
        {
            Server = new Server(IPAddress.Any, port);
        }
    }
}