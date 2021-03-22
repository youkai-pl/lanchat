namespace Lanchat.Core.Models
{
    internal class ConnectionControl
    {
        public ConnectionControlStatus Status { get; init; }
    }

    internal enum ConnectionControlStatus
    {
        RemoteClose
    }
}