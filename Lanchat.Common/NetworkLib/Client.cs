using Lanchat.Common.NetworkLib;
using Lanchat.Common.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.HostLib
{
    internal class Client
    {
        private readonly Node node;

        private NetworkStream stream;

        public TcpClient TcpClient;

        internal Client(Node node)
        {
            this.node = node;
        }

        internal void Connect(IPAddress ip, int port)
        {
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

        internal void ResumeConnection()
        {
            Send("request", "nickname");
        }

        internal void Send(string type, JToken content)
        {
            try
            {
                var data = new JObject(new JProperty(type, content));
                byte[] bytesToSend = Encoding.UTF8.GetBytes(data.ToString());
                stream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Data send failed: {e.Message}");
            }
        }

        internal void SendHandshake(Handshake handshake)
        {
            Send("handshake", JToken.FromObject(handshake));
        }

        internal void SendHeartbeat()
        {
            Send("heartbeat", null);
        }

        internal void SendKey(Key key)
        {
            Send("key", JToken.FromObject(key));
        }

        internal void SendList(List<Node> nodes)
        {
            var list = new List<JToken>();

            foreach (var item in nodes)
            {
                list.Add(JToken.FromObject(new ListItem(item.Ip.ToString(), item.Port)));
            }

            Send("list", JToken.FromObject(list));
        }

        internal void SendMessage(string message)
        {
            if (node.State == Status.Ready)
            {
                Send("message", node.SelfAes.Encode(message));
            }
        }

        internal void SendNickname(string nickname)
        {
            if (node.State == Status.Ready)
            {
                Send("nickname", nickname);
            }
        }
    }
}