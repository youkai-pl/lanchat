namespace Lanchat.Core.Models
{
    internal class FileReceiveRequest
    {
        internal string FileName { get; init; }
        internal long PartsCount { get; init; }
    }
}