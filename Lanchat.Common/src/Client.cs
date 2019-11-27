using Newtonsoft.Json.Linq;
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
            var data = new JObject
            {
                { "type", "handshake" },
                { "content", JToken.FromObject(handshake) }
            };

            // Send
            Send(data);
        }

        // Send message
        public void SendMessage(string message)
        {
            var data = new JObject
            {
                { "type", "message" },
                { "content", message }
            };

            // Send
            Send(data);
        }

        // Serialize and send data
        private void Send(JObject data)
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(data.ToString());
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}