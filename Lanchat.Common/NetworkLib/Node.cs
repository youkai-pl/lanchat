using Lanchat.Common.Cryptography;
using Lanchat.Common.Types;
using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Represents network node.
    /// </summary>
    public class Node : IDisposable
    {
        /// <summary>
        /// Full node constructor.
        /// </summary>
        /// <param name="id">Node ID</param>
        /// <param name="port">Node TCP port</param>
        /// <param name="ip">Node IP</param>
        internal Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
        }

        /// <summary>
        /// Node constructor without id.
        /// </summary>
        /// <param name="port">Node TCP port</param>
        /// <param name="ip">Node IP</param>
        internal Node(int port, IPAddress ip)
        {
            Port = port;
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
        }

        /// <summary>
        /// Node constructor with socket.
        /// </summary>
        internal Node(Socket socket, IPAddress ip)
        {
            Socket = socket;
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
        }

        // Events
        internal event EventHandler StateChanged;
        internal event EventHandler HandshakeAccepted;

        internal Socket Socket { get; set; }

        /// <summary>
        /// Nickname without number.
        /// </summary>
        public string ClearNickname { get; private set; }

        /// <summary>
        /// Heartbeat counter.
        /// </summary>
        public int HearbeatCount { get; set; } = 0;

        /// <summary>
        /// Last heartbeat status.
        /// </summary>
        public bool Heartbeat { get; set; }

        /// <summary>
        /// Node ID.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Node public RSA key.
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Node <see cref="Status"/>.
        /// </summary>
        public Status State { get; set; }

        internal Client Client { get; set; }
        internal Timer HeartbeatTimer { get; set; }
        internal int NicknameNum { get; set; }
        internal Aes RemoteAes { get; set; }
        internal Aes SelfAes { get; set; }

        // Use values from received handshake
        internal void AcceptHandshake(Handshake handshake)
        {
            Nickname = handshake.Nickname;
            PublicKey = handshake.PublicKey;

            if (Id == null)
            {
                Id = handshake.Id;
            }

            if (Port == 0)
            {
                Port = handshake.Port;
                CreateConnection();
                OnHandshakeAccepted();
            }

            Client.SendKey(new Key(
                     Rsa.Encode(SelfAes.Key, PublicKey),
                     Rsa.Encode(SelfAes.IV, PublicKey)));
        }

        // Create connection
        internal void CreateConnection()
        {
            Client = new Client(this);
            Client.Connect(Ip, Port);
        }

        // Create AES instance with received key
        internal void CreateRemoteAes(string key, string iv)
        {
            RemoteAes = new Aes(key, iv);

            // Set ready to true
            State = Status.Ready;
            OnStateChange();

            // Start heartbeat
            StartHeartbeat();
        }

        // Start heartbeat
        internal void StartHeartbeat()
        {
            // Create heartbeat timer
            HeartbeatTimer = new Timer
            {
                Interval = 1200,
                Enabled = true
            };
            HeartbeatTimer.Elapsed += new ElapsedEventHandler(OnHeartebatOver);
            HeartbeatTimer.Start(); ;

            // Start sending heartbeat
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

        /// <summary>
        /// State change event.
        /// </summary>
        protected void OnStateChange()
        {
            StateChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handshake accepted event.
        /// </summary>
        protected void OnHandshakeAccepted()
        {
            HandshakeAccepted(this, EventArgs.Empty);
        }

        // Hearbeat over event
        private void OnHeartebatOver(object o, ElapsedEventArgs e)
        {
            // If heartbeat was not received make count negative
            if (Heartbeat)
            {
                // Reset heartbeat
                Heartbeat = false;

                // Count heartbeat
                if (HearbeatCount < 0)
                {
                    HearbeatCount = 1;
                }
                else
                {
                    HearbeatCount++;
                }

                // Change state
                if (State == Status.Suspended)
                {
                    State = Status.Resumed;
                    OnStateChange();
                }
            }
            else
            {
                // Count heartbeat
                if (HearbeatCount > 0)
                {
                    HearbeatCount = -1;
                }
                else
                {
                    HearbeatCount--;
                }

                // Change state
                if (State != Status.Suspended)
                {
                    State = Status.Suspended;
                    OnStateChange();
                }
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
                    HeartbeatTimer.Dispose();
                    Client.Dispose();
                }
                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}