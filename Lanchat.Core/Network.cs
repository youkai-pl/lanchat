using System.Net;

namespace Lanchat.Core
{
    public class Network
    {
        public Server Server { get;}
        
        public Network(int port)
        {
            Server = new Server(IPAddress.Any, port);
        }
    }
}