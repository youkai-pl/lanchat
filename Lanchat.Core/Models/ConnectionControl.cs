namespace Lanchat.Core.Models
{
    public class ConnectionControl
    {
        public ConnectionControlStatus Status { get; set; }
    }

    public enum ConnectionControlStatus
    {
        RemoteClose
    }
}