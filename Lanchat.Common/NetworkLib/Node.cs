using Lanchat.Common.Cryptography;
using Lanchat.Common.NetworkLib.InternalEvents;
using Lanchat.Common.NetworkLib.Handlers;
using Lanchat.Common.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Represents network node.
    /// </summary>
    public class Node : IDisposable
    {
        /// <summary>
        /// Node constructor with known port.
        /// </summary>
        /// <param name="ip">Node IP</param>
        /// <param name="network">Network</param>
        internal Node(IPAddress ip, Network network)
        {
            ConnectionTimer = new Timer { Interval = 10000, Enabled = false };
            HeartbeatTimer = new Timer { Interval = network.HeartbeatTimeout, Enabled = false };
            Events = new NodeEvents();
            EventsHandlers = new NodeEventsHandlers(network, this);
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
            ConnectionTimer.Start();
        }

        /// <summary>
        /// Nickname without number.
        /// </summary>
        public string ClearNickname { get; private set; }

        /// <summary>
        /// Handshake.
        /// </summary>
        public Handshake Handshake { get; set; }

        /// <summary>
        /// Last heartbeat status.
        /// </summary>
        public bool Heartbeat { get; set; }

        /// <summary>
        /// Node IP.
        /// </summary>
        public IPAddress Ip { get; set; }

        /// <summary>
        /// Node mute value.
        /// </summary>
        public bool Mute { get; set; }

        /// <summary>
        /// Node nickname. If nicknames are duplicated returns nickname with number.
        /// </summary>
        public string Nickname
        {
            get
            {
                if (NicknameNum != 0)
                {
                    return ClearNickname + $"#{NicknameNum}";
                }
                else
                {
                    return ClearNickname;
                }
            }
            set => ClearNickname = value;
        }

        /// <summary>
        /// Node TCP port.
        /// </summary>
        public int Port { get; set; }

        private Status _State;

        /// <summary>
        /// Node <see cref="Status"/>.
        /// </summary>
        public Status State
        {
            get { return _State; }
            set
            {
                var previousState = State;
                _State = value;
                if (previousState != value)
                {
                    Events.OnStateChange();
                }
            }
        }

        internal Client Client { get; set; }
        internal Timer ConnectionTimer { get; set; }
        internal NodeEvents Events { get; set; }
        internal NodeEventsHandlers EventsHandlers { get; set; }
        internal Timer HeartbeatTimer { get; set; }
        internal int NicknameNum { get; set; }
        internal Aes RemoteAes { get; set; }
        internal Aes SelfAes { get; set; }
        internal Socket Socket { get; set; }

        internal void AcceptHandshake(Handshake handshake)
        {
            Handshake = handshake;
            Nickname = handshake.Nickname;

            if (Port == 0)
            {
                Port = handshake.Port;
                CreateConnection();
                Events.OnHandshakeAccepted();
            }

            Client.SendKey(new Key(
                     Rsa.Encode(SelfAes.Key, Handshake.PublicKey),
                     Rsa.Encode(SelfAes.IV, Handshake.PublicKey)));
        }

        internal void CreateConnection()
        {
            Client = new Client(this);
            Client.Connect(Ip, Port);
        }

        internal void CreateRemoteAes(string key, string iv)
        {
            RemoteAes = new Aes(key, iv);
            Activate();
        }

        internal void Process()
        {
            byte[] response;

            while (true)
            {
                response = new byte[Socket.ReceiveBufferSize];
                _ = Socket.Receive(response);

                if (!Socket.IsConnected())
                {
                    Trace.WriteLine($"[HOST] Socket closed ({Ip})");
                    Socket.Close();
                    Events.OnNodeDisconnected(Ip);
                    break;
                }

                try
                {
                    var respBytesList = new List<byte>(response);
                    var data = Encoding.UTF8.GetString(respBytesList.ToArray());

                    // Parse jsons
                    IList<JObject> buffer = new List<JObject>();

                    JsonTextReader reader = new JsonTextReader(new StringReader(data))
                    {
                        SupportMultipleContent = true
                    };

                    using (reader)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                break;
                            }

                            JsonSerializer serializer = new JsonSerializer();
                            JObject packet = serializer.Deserialize<JObject>(reader);

                            buffer.Add(packet);
                        }
                    }

                    // Process all parsed jsons from buffer
                    foreach (JObject packet in buffer)
                    {
                        IList<JToken> obj = packet;
                        var type = ((JProperty)obj[0]).Name;
                        var content = ((JProperty)obj[0]).Value;

                        if(type != "heartbeat")
                        {
                            Trace.WriteLine($"[NODE] Received data ({type})({Ip})");
                        }

                        // Events

                        if (type == "handshake")
                        {
                            Events.OnReceivedHandshake(content.ToObject<Handshake>());
                        }

                        if (type == "key")
                        {
                            Events.OnReceivedKey(content.ToObject<Key>());
                        }

                        if (type == "heartbeat")
                        {
                            Events.OnReceivedHeartbeat();
                        }

                        // Data

                        if (type == "message")
                        {
                            Events.OnReceivedMessage(content.ToString());
                        }

                        if (type == "private")
                        {
                            Events.OnReceivedPrivateMessage(content.ToString());
                        }

                        if (type == "nickname")
                        {
                            Events.OnChangedNickname(content.ToString());
                        }

                        if (type == "list")
                        {
                            Events.OnReceivedList(content.ToObject<List<ListItem>>(), IPAddress.Parse(((IPEndPoint)Socket.LocalEndPoint).Address.ToString()));
                        }
                    }
                }
                catch (DecoderFallbackException)
                {
                    Trace.WriteLine($"[HOST] Data processing error: utf8 decode gone wrong ({Ip})");
                }
                catch (JsonReaderException)
                {
                    Trace.WriteLine($"([HOST] Data processing error: not vaild json ({Ip})");
                }
            }
        }

        internal void StartHeartbeat()
        {
            HeartbeatTimer.Elapsed += new ElapsedEventHandler(OnHeartebatOver);
            HeartbeatTimer.Start();

            new System.Threading.Thread(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (disposedValue)
                    {
                        break;
                    }
                    else
                    {
                        Client.SendHeartbeat();
                    }
                }
            }).Start();
        }

        internal void StartProcess()
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    Process();
                }
                catch (SocketException)
                {
                    // Disconnect node on exception
                    Trace.WriteLine($"[HOST] Socket exception. Node will be disconnected ({Ip})");
                    Events.OnNodeDisconnected(Ip);
                    Socket.Close();
                }
            }).Start();
        }

        private void Activate()
        {
            State = Status.Ready;
            StartHeartbeat();
        }

        private void OnHeartebatOver(object o, ElapsedEventArgs e)
        {
            if (Heartbeat)
            {
                Heartbeat = false;
                if (State == Status.Suspended)
                {
                    State = Status.Resumed;
                }
                else
                {
                    State = Status.Ready;
                }
            }
            else
            {
                State = Status.Suspended;
            }
        }

        /// <summary>
        /// Send private message.
        /// </summary>
        /// <param name="message">content</param>
        public void SendPrivate(string message)
        {
            Client.SendPrivate(message);
        }

        // Dispose

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Node()
        {
            Dispose(false);
        }

        /// <summary>
        /// Node dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Node dispose.
        /// </summary>
        /// <param name="disposing"> Free any other managed objects</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (HeartbeatTimer != null)
                    {
                        HeartbeatTimer.Dispose();
                    }
                    if (Client != null)
                    {
                        Client.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}