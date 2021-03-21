namespace Lanchat.Core.Models
{
    internal class KeyInfo
    {
        public byte[] AesKey { get; set; }
        public byte[] AesIv { get; set; }
    }
}