namespace Lanchat.Core.Encryption
{
    internal interface IBytesEncryption
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);
    }
}