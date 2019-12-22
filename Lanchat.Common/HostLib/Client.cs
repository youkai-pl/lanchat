using Lanchat.Common.HostLib.Types;
using Lanchat.Common.NetworkLib;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.HostLib
{
    public class Client
    {
        public Client(Node node)
        {
            this.node = node;
        }

        // Fields
        private readonly Node node;

        // Tcp client
        private TcpClient client;
        private NetworkStream stream;

        // Connect
        public void Connect(IPAddress ip, int port)
        {
            // Create client and stream
            try
            {
                client = new TcpClient(ip.ToString(), port);
                stream = client.GetStream();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.GetType());
            }
        }

        // Send handshake
        public void SendHandshake(Handshake handshake)
        {
            Send("handshake", JToken.FromObject(handshake));
        }

        // Send key
        public void SendKey(Key key)
        {
            Send("key", JToken.FromObject(key));
        }

        // Send message
        public void SendMessage(string message)
        {
            Send("message", node.SelfAes.Encode(message));
        }

        // Change nickname
        public void SendNickname(string nickname)
        {
            Send("nickname", nickname);
        }

        // Serialize and send data
        private void Send(string type, JToken content)
        {
            var data = new JObject(new JProperty(type, content));

            try
            {
                byte[] bytesToSend = Encoding.UTF8.GetBytes(data.ToString());
                stream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch
            {
                Trace.WriteLine("Data send failed");
            }
        }

        // Send heartbeet
        public void Heartbeat()
        {
            Send("heartbeat", "test");
            Trace.WriteLine("heartbeat sended");
        }

        // Send random data (only for debug)
        public void DestroyLanchat()
        {
            var content = "asdasd";
            var data = new JObject(new JProperty("message", node.SelfAes.Encode(content)));
            byte[] bytesToSend = Encoding.UTF8.GetBytes(data.ToString());
            stream.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}