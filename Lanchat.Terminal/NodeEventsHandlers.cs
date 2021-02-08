using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public class NodeEventsHandlers
    {
        private readonly Node node;

        public NodeEventsHandlers(Node node)
        {
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.NetworkInput.PrivateMessageReceived += OnPrivateMessageReceived;
            node.NetworkInput.PongReceived += OnPongReceived;
            node.FilesExchange.FileExchangeRequestReceived += OnFilesExchangeRequestReceived;
            node.FilesExchange.FileReceived += OnFilesReceived;
            node.FilesExchange.FileExchangeError += OnFileExchangeError;
            
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
            node.SocketErrored += OnSocketErrored;
            node.CannotConnect += OnCannotConnect;
            node.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                    var status = node.Status switch
                    {
                        Status.Online => "online",
                        Status.AwayFromKeyboard => "afk",
                        Status.DoNotDisturb => "dnd",
                        _ => ""
                    };
                    Ui.Log.Add(string.Format(Resources.Info_StatusChange, node.Nickname, status));
                    break;

                case "Nickname":
                    if (!node.Ready) return;
                    Ui.Log.Add(string.Format(Resources.Info_NicknameChanged, node.PreviousNickname, node.Nickname));
                    break;
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources.Info_Connected, node.Nickname));
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources.Info_Reconnecting, node.Nickname));
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources.Info_Disconnected, node.Nickname));
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.Nickname);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
            Ui.Log.AddPrivateMessage(e, node.Nickname);
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Ui.Log.Add(string.Format(Resources.Info_ConnectionError, node.Endpoint.Address, e));
        }

        private void OnCannotConnect(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources.Info_CannotConnect, node.Endpoint));
        }

        private void OnPongReceived(object sender, TimeSpan? e)
        {
            if (e != null)
                Ui.Log.Add(string.Format(Resources.Info_Ping, node.Nickname, e.Value.Milliseconds));
        }
        
        private void OnFilesExchangeRequestReceived(object sender, FileExchangeRequest e)
        {
            Ui.Log.Add(string.Format(Resources.Info_FileRequest, node.Nickname));
        }
        
        private void OnFilesReceived(object sender, FileExchangeRequest e)
        {
            Ui.Log.Add(string.Format(Resources.Info_FileReceived, node.Nickname, e.FilePath));
        }
        
        private void OnFileExchangeError(object sender, Exception e)
        {
            Ui.Log.Add(string.Format(Resources.Info_FileExchangeError, e.Message));
        }
    }
}