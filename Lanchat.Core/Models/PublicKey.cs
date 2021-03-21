namespace Lanchat.Core.Models
{
    internal class PublicKey
    {
        public byte[] RsaModulus { get; set; }
        public byte[] RsaExponent { get; set; }
    }
}