namespace Lanchat.Core.Chat
{
    internal interface IInternalMessaging
    {
        void OnMessageReceived(string content);
        void OnPrivateMessageReceived(string content);
    }
}