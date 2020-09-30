namespace Lanchat.Core.Network
{
    public interface INetworkElement
    {
        bool SendAsync(string text);
    }
}