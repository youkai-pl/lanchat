namespace Lanchat.Core.Models
{
    internal class ConnectionControl
    {
        public ConnectionControlStatus Status { get; set; }
    }

    internal enum ConnectionControlStatus
    {
        RemoteClose
    }
}