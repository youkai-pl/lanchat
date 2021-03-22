namespace Lanchat.Core.Models
{
    internal class FilePart
    {
        public byte[] Data { get; init; }
        public bool Last { get; set; }
    }
}