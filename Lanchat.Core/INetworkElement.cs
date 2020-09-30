namespace Lanchat.Core
{
    public interface INetworkElement
    {
         bool SendAsync(string text);
    }
}