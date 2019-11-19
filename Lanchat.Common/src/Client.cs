using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lanchat.Common.ClientLib
{
    public class Client
    {
        // Listen broadcast
        public void ListenBroadcast(UdpClient udpClient)
        {
            Task.Run(() =>
            {
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    var sender = from.Address;

                    // Try parse
                    try
                    {
                        JObject paperplane = JObject.Parse(Encoding.UTF8.GetString(recvBuffer));
                        if (paperplane["port"] != null && paperplane["id"] != null)
                        {
                            RecievedBroadcast(paperplane, EventArgs.Empty);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("Paperplane parsing error: " + e.Message);
                    }
                }
            });
        }

        // Client events
        public delegate void ClientEventHandler(object o, EventArgs e);
        public event ClientEventHandler RecievedBroadcast;

        // Recieved broadcast
        protected virtual void OnRecievedBroadcast(object o, IPAddress sender, EventArgs e) => RecievedBroadcast(o, EventArgs.Empty);
    }
}
