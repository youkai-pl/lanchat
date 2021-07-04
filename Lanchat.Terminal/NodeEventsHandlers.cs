using System;
using System.IO;
using System.Net.Sockets;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public class NodeEventsHandlers
    {
        private readonly INode node;

        public NodeEventsHandlers(INode node)
        {
            this.node = node;

            node.User.NicknameUpdated += OnNicknameUpdated;
            node.User.StatusUpdated += OnStatusUpdated;

            node.Messaging.MessageReceived += OnMessageReceived;
            node.Messaging.PrivateMessageReceived += OnPrivateMessageReceived;

            node.FileReceiver.FileReceiveFinished += OnFileReceiveFinished;
            node.FileReceiver.FileTransferError += OnFileTransferError;
            node.FileReceiver.FileTransferRequestReceived += OnFileTransferHandlerRequestReceived;

            node.FileSender.AcceptedByReceiver += OnFileTransferHandlerReceiveAcceptedByReceiver;
            node.FileSender.FileTransferRequestRejected += OnFileTransferHandlerRequestRejected;
            node.FileSender.FileSendFinished += OnFileSendFinished;
            node.FileSender.FileTransferError += OnFileTransferError;

            node.FileSender.AcceptedByReceiver += Ui.FileTransferMonitor.OnAcceptedByReceiver;
            node.FileSender.FileSendFinished += Ui.FileTransferMonitor.OnFileReceiveFinished;
            node.FileSender.FileTransferError += Ui.FileTransferMonitor.OnFileTransferError;
            node.FileReceiver.FileReceiveFinished += Ui.FileTransferMonitor.OnFileReceiveFinished;
            node.FileReceiver.FileTransferError += Ui.FileTransferMonitor.OnFileTransferError;

            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.SocketErrored += OnSocketErrored;
        }

        private void OnNicknameUpdated(object sender, string e)
        {
            Ui.Log.Add(string.Format(Resources._NicknameChanged, node.User.PreviousNickname, e));
        }

        private void OnStatusUpdated(object sender, UserStatus e)
        {
            var status = e switch
            {
                UserStatus.Online => "online",
                UserStatus.AwayFromKeyboard => "afk",
                UserStatus.DoNotDisturb => "dnd",
                _ => ""
            };
            Ui.Log.Add(string.Format(Resources._StatusChange, node.User.Nickname, status));
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources._Connected, node.User.Nickname));
            Ui.BottomBar.NodesCount.Text = Program.Network.Nodes.Count.ToString();

            switch (node.NodeRsa.KeyStatus)
            {
                case KeyStatus.ChangedKey:
                    Ui.Log.AddError("Public key has changed. Connection may be not secure.");
                    Ui.Log.AddError(node.NodeRsa.PublicPem);
                    break;

                case KeyStatus.FreshKey:
                    Ui.Log.AddWarning("To make sure the connection is secure check the public key:");
                    Ui.Log.AddWarning(node.NodeRsa.PublicPem);
                    break;
            }
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources._Disconnected, node.User.Nickname));
            Ui.BottomBar.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.User.Nickname, false);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.User.Nickname, true);
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Ui.Log.AddError(string.Format(Resources._ConnectionError, node.Id, e));
        }

        private void OnFileTransferHandlerRequestReceived(object sender, CurrentFileTransfer e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequest, node.User.Nickname, e.FileName));
        }

        private void OnFileReceiveFinished(object sender, CurrentFileTransfer e)
        {
            Ui.Log.Add(string.Format(Resources._FileReceived, node.User.Nickname, Path.GetFullPath(e.FilePath)));
        }

        private void OnFileSendFinished(object sender, CurrentFileTransfer e)
        {
            Ui.Log.Add(string.Format(Resources._FileTransferFinished, node.User.Nickname));
        }

        private void OnFileTransferError(object sender, FileTransferException e)
        {
            Ui.Log.AddError(string.Format(Resources._FileTransferError, node.User.Nickname));
        }

        private void OnFileTransferHandlerReceiveAcceptedByReceiver(object sender, CurrentFileTransfer e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequestAccepted, node.User.Nickname));
        }

        private void OnFileTransferHandlerRequestRejected(object sender, CurrentFileTransfer e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequestRejected, node.User.Nickname));
        }
    }
}