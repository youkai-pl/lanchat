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
        IConnection Connection { get; set; }
        IInternalUser InternalUser { get; set; }
        IUser User { get; set; }
        IHost Host { get; set; }
        IFileReceiver FileReceiver { get; set; }
        IFileSender FileSender { get; set; }
        IMessaging Messaging { get; set; }
        IOutput Output { get; set; }
        IInput Input { get; set; }
        IInternalNodeRsa InternalNodeRsa { get; set; }

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