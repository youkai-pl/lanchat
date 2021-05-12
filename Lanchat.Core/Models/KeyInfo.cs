namespace Lanchat.Core.Models
{
    internal class KeyInfo
    {
        internal byte[] AesKey { get; init; }
        internal byte[] AesIv { get; init; }
    }
}