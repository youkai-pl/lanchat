using Lanchat.Common.NetworkLib;
using Lanchat.Common.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.HostLib
{
    internal class Client : IDisposable
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
            var data = new JObject(new JProperty(type, content));
            byte[] bytesToSend = Encoding.UTF8.GetBytes(data.ToString());

            try
            {
                stream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch (IOException)
            {
                Trace.WriteLine($"[CLIENT] Socket error ({node.Ip})");
            }
            catch (InvalidOperationException)
            {
                Trace.WriteLine($"[CLIENT] NetworkStream error ({node.Ip})");
            }
        }

        internal void SendHandshake(Handshake handshake)
        {
            Send("handshake", JToken.FromObject(handshake));
            Trace.WriteLine($"[CLIENT] Handshake sent ({node.Ip})");
        }

        internal void SendHeartbeat()
        {
            Send("heartbeat", null);
        }

        internal void SendKey(Key key)
        {
            Send("key", JToken.FromObject(key));
            Trace.WriteLine($"[CLIENT] Aes key sent ({node.Ip})");
        }

        internal void SendList(List<Node> nodes)
        {
            var list = new List<JToken>();

            foreach (var item in nodes)
            {
                list.Add(JToken.FromObject(new ListItem(item.Ip.ToString(), item.Port)));
            }

            Send("list", JToken.FromObject(list));
            Trace.WriteLine($"[CLIENT] Nodes list sent ({node.Ip})");
        }

        internal void SendMessage(string message)
        {
            if (node.State == Status.Ready)
            {
                Send("message", node.SelfAes.Encode(message));
                Trace.WriteLine($"[CLIENT] Messag sent ({node.Ip} / {message})");
            }
            else
            {
                Trace.WriteLine($"[CLIENT] Message not sent. Node isn't ready ({node.Ip} / {message})");
            }
        }

        internal void SendNickname(string nickname)
        {
            if (node.State == Status.Ready)
            {
                Send("nickname", nickname);
                Trace.WriteLine($"[CLIENT] Nickname sent ({node.Ip})");
            }
            else
            {
                Trace.WriteLine($"[CLIENT] Nickname not sent. Node isn't ready ({node.Ip})");
            }
        }

        public void Dispose()
        {
            stream.Dispose();
            TcpClient.Dispose();
        }
    }
}