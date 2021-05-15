namespace Lanchat.Core.Chat.Models
{
    internal interface IInternalMessaging
    {
        void OnMessageReceived(string content);
        void OnPrivateMessageReceived(string content);
    }
}