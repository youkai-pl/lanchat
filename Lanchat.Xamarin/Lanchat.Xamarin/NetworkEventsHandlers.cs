using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using Lanchat.Xamarin.ViewModels;

namespace Lanchat.Xamarin
{
    public class NetworkEventsHandlers
    {
        private readonly ChatViewModel chatViewModel;

        public NetworkEventsHandlers(ChatViewModel chatViewModel)
        {
            this.chatViewModel = chatViewModel;
        }

        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            chatViewModel.AddMessage(new Message() { Content = $"{e.OldNickname} changed nickname to {e.NewNickname}", Nickname = "SYSTEM" });
        }

        public void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage(new Message() { Content = $"{e.Node.Nickname} connected", Nickname = "SYSTEM" });
        }

        public void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage(new Message() { Content = $"{e.Node.Nickname} disconnected", Nickname = "SYSTEM" });
        }

        public void OnNodeResumed(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage(new Message() { Content = $"{e.Node.Nickname} reconnected", Nickname = "SYSTEM" });
        }

        public void OnNodeSuspended(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage(new Message() { Content = $"{e.Node.Nickname} suspended. Waiting for reconnect", Nickname = "SYSTEM" });
        }

        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (e.Target == MessageTarget.Private)
            {
                chatViewModel.Messages.Add(new Message() { Content = e.Content.Trim(), Nickname = $"[->] {e.Node.Nickname}" });
            }
            else
            {
                chatViewModel.Messages.Add(new Message() { Content = e.Content.Trim(), Nickname = e.Node.Nickname });
            }
        }
    }
}