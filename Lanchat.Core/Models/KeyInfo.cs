namespace Lanchat.Core.Models
{
    public class KeyInfo
    {
        public byte[] AesKey { get; set; }
        public byte[] AesIv { get; set; }
    }
}