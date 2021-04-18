using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.API;
using Lanchat.Core.Chat;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    /// <summary>
    ///     Connected user.
    /// </summary>
    public interface INodePublic : INotifyPropertyChanged
    {
        /// <summary>
        ///     Node user nickname.
        /// </summary>
        string Nickname { get; }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        string PreviousNickname { get; }

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Short ID.
        /// </summary>
        string ShortId { get; }
        
        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        bool Ready { get; }

        /// <summary>
        ///     Node user status.
        /// </summary>
        Status Status { get; }
        
        /// <see cref="INetworkElement" />
        INetworkElement NetworkElement { get; }

        /// <see cref="Messaging" />
        Messaging Messaging { get; }

        /// <see cref="Output" />
        IOutput Output { get; }

        /// <see cref="FileReceiver" />
        FileReceiver FileReceiver { get; }

        /// <see cref="FileSender" />
        FileSender FileSender { get; }
        
        /// <see cref="Lanchat.Core.API.Resolver" />
        Resolver Resolver { get; }

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