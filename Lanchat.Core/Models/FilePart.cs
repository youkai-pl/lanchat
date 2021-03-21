namespace Lanchat.Core.Models
{
    internal class FilePart
    {
        public byte[] Data { get; set; }
        public bool Last { get; set; }
    }
}