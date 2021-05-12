namespace Lanchat.Core.Encryption
{
    internal class PublicKey
    {
        public byte[] RsaModulus { get; init; }
        public byte[] RsaExponent { get; init; }
    }
}