using Lanchat.Common.Cryptography;
using Lanchat.Common.Types;
using System;
using System.Diagnostics;
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
        /// Node constructor with known port.
        /// </summary>
        /// <param name="ip">Node IP</param>
        internal Node(IPAddress ip)
        {
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
            HandshakeTimer = new Timer { Interval = 5000, Enabled = true };
            HeartbeatTimer = new Timer { Interval = 1200, Enabled = false };
            WaitForHandshake();
        }

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
        public Status State { get; set; }

        /// <summary>
        /// Handshake.
        /// </summary>
        public Handshake Handshake { get; set; }

        internal Client Client { get; set; }
        internal Timer HeartbeatTimer { get; set; }
        internal Timer HandshakeTimer { get; set; }
        internal int NicknameNum { get; set; }
        internal Aes RemoteAes { get; set; }
        internal Aes SelfAes { get; set; }
        internal Socket Socket { get; set; }
        internal NodeEventsHandlers EventsHandlers { get; set; }

        internal event EventHandler HandshakeAccepted;
        internal event EventHandler StateChanged;

        internal void AcceptHandshake(Handshake handshake)
        {
            Handshake = handshake;
            Nickname = handshake.Nickname;

            if (Port == 0)
            {
                Port = handshake.Port;
                CreateConnection();
                OnHandshakeAccepted();
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

            State = Status.Ready;
            OnStateChange();

            StartHeartbeat();
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

        internal void WaitForHandshake()
        {
            // Wait for handshake
            HandshakeTimer.Start();
        }

        /// <summary>
        /// Handshake accepted event.
        /// </summary>
        protected void OnHandshakeAccepted()
        {
            HandshakeAccepted(this, EventArgs.Empty);
        }

        /// <summary>
        /// State change event.
        /// </summary>
        protected void OnStateChange()
        {
            StateChanged(this, EventArgs.Empty);
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