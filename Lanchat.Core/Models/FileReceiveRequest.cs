namespace Lanchat.Core.Models
{
    internal class FileReceiveRequest
    {
        public string FileName { get; init; }
        public long PartsCount { get; init; }
    }
}