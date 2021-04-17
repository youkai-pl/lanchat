namespace Lanchat.Core.Models
{
    public class KeyInfo
    {
        public byte[] AesKey { get; init; }
        public byte[] AesIv { get; init; }
    }
}