namespace Lanchat.Core.Models
{
    public class FilePart
    {
        public byte[] Data { get; set; }
        public bool Last { get; set; }
    }
}