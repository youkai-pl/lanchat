using System;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal interface INodeInternal
    {
        public Connection Connection { get; set; }
        public IInternalUser InternalUser { get; set; }
        public IUser User { get; set; }
        public IHost Host { get; set; }
        public IFileReceiver FileReceiver { get; set; }
        public IFileSender FileSender { get; set; }
        public IMessaging Messaging { get; set; }
        public IOutput Output { get; set; }
        public IInput Input { get; set; }
        public IInternalNodeRsa InternalNodeRsa { get; set; }

        Guid Id { get; }
        bool Ready { get; set; }
        void Start();
        void OnConnected();
        void OnDisconnected();
        void OnCannotConnect();

        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler CannotConnect;
    }
}