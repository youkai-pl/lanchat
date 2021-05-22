using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    /// <summary>
    ///     Connected user.
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        bool Ready { get; }
        
        /// <see cref="IUser"/>
        IUser User { get; }

        /// <see cref="IHost" />
        IHost Host { get; }

        /// <see cref="Lanchat.Core.Chat.IMessaging" />
        IMessaging Messaging { get; }

        /// <see cref="Lanchat.Core.FileTransfer.FileReceiver" />
        IFileReceiver FileReceiver { get; }

        /// <see cref="Lanchat.Core.FileTransfer.FileSender" />
        IFileSender FileSender { get; }

        /// <see cref="Lanchat.Core.Api.IOutput" />
        IOutput Output { get; }
        
        /// <summary>
        ///     Node successful connected and ready to data exchange.
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        ///     TCP session or client returned error.
        /// </summary>
        event EventHandler<SocketError> SocketErrored;

        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        void Disconnect();
    }
}