namespace Lanchat.Core.Encryption
{
    internal interface IStringEncryption
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}