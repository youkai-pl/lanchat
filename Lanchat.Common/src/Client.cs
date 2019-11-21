using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.HostLib
{
    public class Client
    {
        // Tcp client
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        // Connect
        public void Connect(IPAddress ip, int port, Handshake handshake)
        {
            // Create client and stream
            tcpclnt = new TcpClient(ip.ToString(), port);
            nwStream = tcpclnt.GetStream();

            // Send handshake
            byte[] bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(handshake));
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        // Client events
        public delegate void ClientEventHandler(params object[] arguments);
    }
}
