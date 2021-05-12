namespace Lanchat.Core.Models
{
    internal class PublicKey
    {
        internal byte[] RsaModulus { get; init; }
        internal byte[] RsaExponent { get; init; }
    }
}