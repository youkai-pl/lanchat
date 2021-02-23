namespace Lanchat.Core.Models
{
    public class PublicKey
    {
        public byte[] RsaModulus { get; set; }
        public byte[] RsaExponent { get; set; }
    }
}