using Lanchat.Common.NetworkLib.Exceptions;
using Lanchat.Common.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.NetworkLib.Node
{
    internal class Client : IDisposable
    {
        private readonly NodeInstance node;

        private NetworkStream stream;

        internal TcpClient TcpClient;

        internal Client(NodeInstance node)
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
            catch (NullReferenceException)
            {
                Trace.WriteLine($"[CLIENT] Stream closed error ({node.Ip})");
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

        internal void SendList(List<NodeInstance> nodes)
        {
            var list = new List<JToken>();

            foreach (var item in nodes.ToList())
            {
                list.Add(JToken.FromObject(new ListItem(item.Ip.ToString(), item.Port)));
            }

            Send("list", JToken.FromObject(list));
            Trace.WriteLine($"[CLIENT] Nodes list sent ({node.Ip})");
        }

        internal void SendPrivate(string message)
        {
            if (node.State == Status.Ready)
            {
                Send("private", node.SelfAes.Encode(message));
                Trace.WriteLine($"[CLIENT] Private message sent ({node.Ip})");
            }
            else
            {
                Trace.WriteLine($"[CLIENT] Private message not sent. Node isn't ready ({node.Ip})");
            }
        }

        internal void SendMessage(string message)
        {
            if (node.State == Status.Ready)
            {
                Send("message", node.SelfAes.Encode(message));
                Trace.WriteLine($"[CLIENT] Message sent ({node.Ip})");
            }
            else
            {
                Trace.WriteLine($"[CLIENT] Message not sent. Node isn't ready ({node.Ip})");
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
            if (stream != null)
            {
                stream.Close();
            }
            if (TcpClient != null)
            {
                TcpClient.Close();
            }
        }
    }
}