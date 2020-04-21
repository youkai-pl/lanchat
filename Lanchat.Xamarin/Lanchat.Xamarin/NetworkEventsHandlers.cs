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
            chatViewModel.AddMessage($"{e.OldNickname} changed nickname to {e.NewNickname}");
        }

        public void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage($"{e.Node.Nickname} connected");
        }

        public void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage($"{e.Node.Nickname} disconnected");
        }

        public void OnNodeResumed(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage($"{e.Node.Nickname} reconnected");
        }

        public void OnNodeSuspended(object o, NodeConnectionStatusEventArgs e)
        {
            chatViewModel.AddMessage($"{e.Node.Nickname} suspended. Waiting for reconnect");
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