namespace Lanchat.Core.Encryption
{
    public interface IBytesEncryption
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);
    }
}