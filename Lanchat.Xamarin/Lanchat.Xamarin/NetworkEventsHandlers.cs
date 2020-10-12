using Lanchat.Core;
using Lanchat.Xamarin.ViewModels;
using System;

namespace Lanchat.Xamarin
{
    public class NetworkEventsHandlers
    {
        private readonly ChatViewModel chatViewModel;
        private readonly Node node;

        public NetworkEventsHandlers(ChatViewModel chatViewModel, Node node)
        {
            this.chatViewModel = chatViewModel;
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
        }


        private void OnDisconnected(object sender, EventArgs e)
        {
            chatViewModel.AddMessage(new Message { Content = $"{node.Nickname} disconnected. Trying reconnect." });
        }
        
        private void OnHardDisconnected(object sender, EventArgs e)
        {
            chatViewModel.AddMessage(new Message { Content = $"{node.Nickname} disconnected. Cannot reconnect." });
        }

        private void OnConnected(object sender, EventArgs e)
        {
            chatViewModel.AddMessage(new Message { Content = $"{node.Nickname} connected" });
        }

        private void OnMessageReceived(object sender, string e)
        {
            chatViewModel.AddMessage(new Message { Content = e, Nickname = node.Nickname });
        }
    }
}