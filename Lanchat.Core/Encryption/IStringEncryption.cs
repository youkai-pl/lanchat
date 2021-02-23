namespace Lanchat.Core.Encryption
{
    public interface IStringEncryption
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}