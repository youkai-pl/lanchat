using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib;
using Lanchat.Common.HostLib.Types;
using System;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace Lanchat.Common.NetworkLib
{
    public class Node : IDisposable
    {
        public Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
            SelfAes = new AesInstance();
            NicknameNum = 0;
        }

        // Properties
        public string ClearNickname { get; private set; }
        public Client Client { get; set; }
        public bool Heartbeat { get; set; }
        public Timer HeartbeatTimer { get; set; }
        public Guid Id { get; set; }
        public IPAddress Ip { get; set; }
        public bool Mute { get; set; }
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
        public int NicknameNum { get; set; }
        public int Port { get; set; }
        public string PublicKey { get; set; }
        public bool Ready
        {
            get
            {
                return _Ready;
            }
            set
            {
                var prev = _Ready;
                _Ready = value;
                if (_Ready != prev)
                {
                    OnReadyChange();
                }
            }
        }
        public AesInstance RemoteAes { get; set; }
        public AesInstance SelfAes { get; set; }
        public int HearbeatCount { get; set; } = 0;

        // Fields
        private bool _Ready;

        // Ready property change event
        public event EventHandler ReadyChanged;
        protected void OnReadyChange()
        {
            ReadyChanged(this, EventArgs.Empty);
        }

        // Low heatbeat event
        public event EventHandler LowHeartbeat;
        protected void OnLowHeartbeat()
        {
            LowHeartbeat(this, EventArgs.Empty);
        }

        // Use values from received handshake
        public void AcceptHandshake(Handshake handshake)
        {
            Nickname = handshake.Nickname;
            PublicKey = handshake.PublicKey;

            // Send AES encryption key
            Client.SendKey(new Key(
                Rsa.Encode(SelfAes.Key, PublicKey),
                Rsa.Encode(SelfAes.IV, PublicKey)));
        }

        // Create connection
        public void CreateConnection()
        {
            Client = new Client(this);
            Client.Connect(Ip, Port);
        }

        // Create AES instance with received key
        public void CreateRemoteAes(string key, string iv)
        {
            RemoteAes = new AesInstance(key, iv);

            // Set ready to true
            Ready = true;

            // Start heartbeat
            StartHeartbeat();
        }

        // Start heartbeat
        public void StartHeartbeat()
        {
            // Create heartbeat timer
            HeartbeatTimer = new Timer
            {
                Interval = 2500,
                Enabled = true
            };
            HeartbeatTimer.Elapsed += new ElapsedEventHandler(OnHeartebatOver);
            HeartbeatTimer.Start(); ;

            // Start sending heartbeat
            new System.Threading.Thread(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(2000);
                    if (disposedValue)
                    {
                        break;
                    }
                    else
                    {
                        Client.Heartbeat();
                    }
                }
            }).Start();
        }

        // Hearbeat over event
        private void OnHeartebatOver(object o, ElapsedEventArgs e)
        {
            // If heartbeat was not received make count negative
            if (!Heartbeat)
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

                // Change ready state
                Ready = false;
                Trace.WriteLine($"({Ip}) ({HearbeatCount}) heartbeat over");

                // If heartbeat lower than -2 call event
                if (HearbeatCount < -2)
                {
                    OnLowHeartbeat();
                }
            }
            else
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

                // Change ready state
                Ready = true;
                Trace.WriteLine($"({Ip}) ({HearbeatCount}) heartbeat ok");
            }
        }

        // Dispose
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    HeartbeatTimer.Dispose();
                    Client.TcpClient.Dispose();
                }
                disposedValue = true;
            }
        }

        ~Node()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}