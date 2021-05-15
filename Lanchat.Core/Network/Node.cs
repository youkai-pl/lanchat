using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal class Node : IDisposable, INode, INodeInternal
    {
        private string nickname;
        private string previousNickname;

        public Node(IHost host)
        {
            IsSession = host.IsSession;
            Host = host;
            Host.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);
        }

        public Connection Connection { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IHost Host { get; }
        public IResolver Resolver { get; set; }
        public IFileReceiver FileReceiver { get; set; }
        public IFileSender FileSender { get; set; }
        public IMessaging Messaging { get; set; }
        public IOutput Output { get; set; }

        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
            set
            {
                if (value == nickname)
                {
                    return;
                }

                previousNickname = nickname;
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public string PreviousNickname => $"{previousNickname}#{ShortId}";
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);
        public Guid Id => Host.Id;
        public bool Ready { get; set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Disconnect()
        {
            Output.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionStatus.RemoteDisconnect
            });
            Dispose();
        }

        public bool IsSession { get; }

        public void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }

        internal event EventHandler CannotConnect;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}