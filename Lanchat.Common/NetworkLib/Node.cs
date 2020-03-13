using Lanchat.Common.Cryptography;
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
using System.Linq;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Represents network node.
    /// </summary>
    public class Node : IDisposable
    {
        private Status _State;

        /// <summary>
        /// Node constructor with known port.
        /// </summary>
        /// <param name="ip">Node IP</param>
        /// <param name="network">Network</param>
        internal Node(IPAddress ip, Network network)
        {
            ConnectionTimer = new Timer { Interval = 10000, Enabled = false };
            HeartbeatTimer = new Timer { Interval = network.HeartbeatTimeout, Enabled = false };
            Handlers = new NodeHandlers(network, this);
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
                    Handlers.OnStateChanged();
                }
            }
        }

        internal Client Client { get; set; }
        internal Timer ConnectionTimer { get; set; }
        internal NodeHandlers Handlers { get; set; }
        internal Timer HeartbeatTimer { get; set; }
        internal int NicknameNum { get; set; }
        internal Aes RemoteAes { get; set; }
        internal Aes SelfAes { get; set; }
        internal Socket Socket { get; set; }

        /// <summary>
        /// Send private message.
        /// </summary>
        /// <param name="message">content</param>
        public void SendPrivate(string message)
        {
            Client.SendPrivate(message);
        }

        internal void AcceptHandshake(Handshake handshake)
        {
            Handshake = handshake;
            Nickname = handshake.Nickname;

            if (Port == 0)
            {
                Port = handshake.Port;
                CreateConnection();
                Handlers.OnHandshakeAccepted();
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
            byte[] streamBuffer;

            while (Socket.IsConnected())
            {
                // Read stream
                streamBuffer = new byte[Socket.ReceiveBufferSize];
                _ = Socket.Receive(streamBuffer);

                // Get string
                var respBytesList = new List<byte>(streamBuffer);
                var data = Encoding.UTF8.GetString(respBytesList.ToArray());
                
                try
                {
                    JsonTextReader reader = new JsonTextReader(new StringReader(data))
                    {
                        SupportMultipleContent = true
                    };

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            HandleReceivedData(serializer.Deserialize<JObject>(reader));
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

            Trace.WriteLine($"[HOST] Socket closed ({Ip})");
            Socket.Close();
            Handlers.OnNodeDisconnected();
        }

        private void HandleReceivedData(JObject json)
        {
            var type = json.Properties().Select(p => p.Name).FirstOrDefault();
            var content = json.Properties().Select(p => p.Value).FirstOrDefault();

            if (type == "handshake")
            {
                Handlers.OnReceivedHandshake(content.ToObject<Handshake>());
            }

            if (type == "key")
            {
                Handlers.OnReceivedKey(content.ToObject<Key>());
            }

            if (type == "heartbeat")
            {
                Handlers.OnReceivedHeartbeat();
            }

            if (type == "message")
            {
                Handlers.OnReceivedMessage(content.ToString(), MessageTarget.Broadcast);
            }

            if (type == "private")
            {
                Handlers.OnReceivedMessage(content.ToString(), MessageTarget.Private);
            }

            if (type == "nickname")
            {
                Handlers.OnChangedNickname(content.ToString());
            }

            if (type == "list")
            {
                Handlers.OnReceivedList(content.ToObject<List<ListItem>>(), IPAddress.Parse(((IPEndPoint)Socket.LocalEndPoint).Address.ToString()));
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
                    Handlers.OnNodeDisconnected();
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