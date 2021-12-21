using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Handlers
{
    public class NodeHandlers
    {
        private readonly INode node;
        private Tab privateChatTab;
        private ChatView privateChatView;

        public NodeHandlers(INode node)
        {
            this.node = node;
            node.Connected += NodeOnConnected;
            node.Disconnected += NodeOnDisconnected;
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
            node.User.NicknameUpdated += UserOnNicknameUpdated;
            node.User.StatusUpdated += UserOnStatusUpdated;
        }

        private void NodeOnConnected(object sender, EventArgs e)
        {
            TabsManager.ShowMainChatView();
            privateChatTab = TabsManager.AddPrivateChatView(node);
            privateChatView = (ChatView)privateChatTab.Content;
            Writer.WriteStatus(string.Format(Resources.Connected, $"{node.User.Nickname}#{node.User.ShortId}"));
            TabsManager.UsersView.RefreshUsersView();

            UpdateHeaderColor();

            switch (node.NodeRsa.KeyStatus)
            {
                case KeyStatus.FreshKey:
                    Writer.WriteWarning(string.Format(Resources.FreshRsa,
                        $"{node.User.Nickname}#{node.User.ShortId}"));
                    break;

                case KeyStatus.ChangedKey:
                    Writer.WriteError(string.Format(Resources.RsaChanged,
                        $"{node.User.Nickname}#{node.User.ShortId}"));
                    break;
            }

            if(!node.Trusted)
            {
                Writer.WriteWarning(string.Format(Resources.ConnectedWithUntrusted, $"{node.User.Nickname}#{node.User.ShortId}"));
            }
        }

        private void NodeOnDisconnected(object sender, EventArgs e)
        {
            TabsManager.ClosePrivateChatView(node);
            Writer.WriteStatus(string.Format(Resources.Disconnected, $"{node.User.Nickname}#{node.User.ShortId}"));
            TabsManager.UsersView.RefreshUsersView();
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            Program.Notifications.ShowNotification();
            TabsManager.MainChatView.AddMessage(e, $"{node.User.Nickname}#{node.User.ShortId}");
            TabsManager.SignalNewMessage();
        }

        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            Program.Notifications.ShowNotification();
            privateChatView.AddMessage(e, $"{node.User.Nickname}#{node.User.ShortId}");
            TabsManager.SignalPrivateNewMessage(node);
        }

        private void UserOnNicknameUpdated(object sender, string e)
        {
            privateChatTab?.Header.UpdateText($"{node.User.Nickname}#{node.User.ShortId}");
            TabsManager.UsersView.RefreshUsersView();
            Writer.WriteStatus(
                string.Format(Resources.NicknameChanged, node.User.PreviousNickname,
                    $"{node.User.Nickname}#{node.User.ShortId}"));
        }

        private void UserOnStatusUpdated(object sender, UserStatus e)
        {
            UpdateHeaderColor();
        }

        private void UpdateHeaderColor()
        {
            switch (node.User.UserStatus)
            {
                case UserStatus.Online:
                    privateChatTab?.Header.UpdateTextColor(ConsoleColor.White);
                    break;
                case UserStatus.AwayFromKeyboard:
                    privateChatTab?.Header.UpdateTextColor(ConsoleColor.Yellow);
                    break;
                case UserStatus.DoNotDisturb:
                    privateChatTab?.Header.UpdateTextColor(ConsoleColor.Red);
                    break;
            }
        }
    }
}