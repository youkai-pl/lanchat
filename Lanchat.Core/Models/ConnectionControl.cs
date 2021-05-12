using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class ConnectionControl
    {
        [Required] internal ConnectionStatus Status { get; init; }
    }

    internal enum ConnectionStatus
    {
        RemoteDisconnect
    }
}