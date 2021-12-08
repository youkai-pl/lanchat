using System.Net.Sockets;
using System.Text;

namespace Lanchat.Ipc
{
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder StringBuilder = new();
        public Socket Socket;
    }
}