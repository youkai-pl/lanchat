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
            Writer.WriteStatus(string.Format(Resources._Connected, node.User.NicknameWithId));
            TabsManager.UsersView.RefreshUsersView();

            UpdateHeaderColor();

            switch (node.NodeRsa.KeyStatus)
            {
                case KeyStatus.FreshKey:
                    Writer.WriteWarning(string.Format(Resources._FreshRsa, node.User.NicknameWithId));
                    break;

                case KeyStatus.ChangedKey:
                    Writer.WriteError(string.Format(Resources._RsaChanged, node.User.NicknameWithId));
                    break;
            }
        }

        private void NodeOnDisconnected(object sender, EventArgs e)
        {
            TabsManager.ClosePrivateChatView(node);
            Writer.WriteStatus(string.Format(Resources._Disconnected, node.User.NicknameWithId));
            TabsManager.UsersView.RefreshUsersView();
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            TabsManager.MainChatView.AddMessage(e, node.User.NicknameWithId);
            TabsManager.SignalNewMessage();
        }

        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            privateChatView.AddMessage(e, node.User.NicknameWithId);
            TabsManager.SignalPrivateNewMessage(node);
        }

        private void UserOnNicknameUpdated(object sender, string e)
        {
            privateChatTab?.Header.UpdateText(node.User.NicknameWithId);
            TabsManager.UsersView.RefreshUsersView();
            Writer.WriteStatus(
                string.Format(Resources._NicknameChanged, node.User.PreviousNickname, node.User.NicknameWithId));
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