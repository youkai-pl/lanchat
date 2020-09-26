using System.Net;

namespace Lanchat.Core
{
    public class Network
    {
        public Server Server { get;}
        public Events Events { get; }
        
        public Network(int port)
        {
            Events = new Events();
            Server = new Server(IPAddress.Any, port, Events);
        }

        public Client CreateClient(string ipAddress, int clientPort)
        {
            return new Client(ipAddress, clientPort, Events);
        }
    }
}