using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    internal static class SocketExtensions
    {
        internal static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
