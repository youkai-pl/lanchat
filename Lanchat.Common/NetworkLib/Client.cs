using Lanchat.Common.Types;
using Lanchat.Common.NetworkLib;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.HostLib
{
    internal class Client
    {
        // Fields
        private readonly Node node;

        private NetworkStream stream;

        internal Client(Node node)
        {
            this.node = node;
        }

        internal TcpClient TcpClient { get; set; }

        // Connect
        internal void Connect(IPAddress ip, int port)
        {
            // Create client and stream
            try
            {
                TcpClient = new TcpClient(ip.ToString(), port);
                stream = TcpClient.GetStream();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.GetType());
                throw new ConnectionFailedException();
            }
        }

        // Send heartbeet
        internal void Heartbeat()
        {
            Send("heartbeat", null);
        }

        // Serialize and send data
        internal void Send(string type, JToken content)
        {
            // Create json
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

        // Send handshake
        internal void SendHandshake(Handshake handshake)
        {
            Send("handshake", JToken.FromObject(handshake));
        }

        // Send key
        internal void SendKey(Key key)
        {
            Send("key", JToken.FromObject(key));
        }

        // Send message
        internal void SendMessage(string message)
        {
            if (node.State == Status.Ready)
            {
                Send("message", node.SelfAes.Encode(message));
            }
        }

        // Change nickname
        internal void SendNickname(string nickname)
        {
            if (node.State == Status.Ready)
            {
                Send("nickname", nickname);
            }
        }

        // Request nickname
        internal void ResumeConnection()
        {
            Send("request:nickname", null);
        }
    }
}