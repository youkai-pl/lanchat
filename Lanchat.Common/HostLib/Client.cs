using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Lanchat.Common.CryptographyLib;

namespace Lanchat.Common.HostLib
{
    public class Client
    {
        // Tcp client
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        // Connect
        public void Connect(IPAddress ip, int port)
        {
            // Create client and stream
            tcpclnt = new TcpClient(ip.ToString(), port);
            nwStream = tcpclnt.GetStream();
        }

        // Send handshake
        public void SendHandshake(Handshake handshake)
        {
            // Send handshake
            var data = new JObject
            {
                { "type", "handshake" },
                { "content", JToken.FromObject(handshake) }
            };

            // Send
            Send(data);
        }

        // Send key
        public void SendKey(string remoteKey, string symetricKey)
        {
            var encodedSymetricKey = Rsa.Encode(symetricKey, remoteKey);
            var data = new JObject
            {
                {"type", "key" },
                {"content", encodedSymetricKey}
            };
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

        // Change nickname
        public void SendNickname(string nickname)
        {
            var data = new JObject
            {
                { "type", "nickname" },
                { "content", nickname }
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