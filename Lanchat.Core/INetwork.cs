namespace Lanchat.Core
{
    public interface INetwork
    {
        void BroadcastMessage(string message);
    }
}